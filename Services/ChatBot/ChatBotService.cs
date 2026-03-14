using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.ChatBotAIEntities;
using Domain.Entities.ChatBotAIEntities.Enums;
using Domain.Exceptions.BadRequest;
using Domain.Exceptions.NotFound;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Services.Abstractions.ChatBot;
using Services.Specifications.ChatBot;
using Shared.Dtos.ChatBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos.ChatBot.Enums;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Services.ChatBot
{
    public class ChatBotService(IUnitOfWork _unitOfWork,
                                IMapper _mapper,
                                IHttpClientFactory _httpClientFactory,
                                ILogger<ChatBotService> _logger) : IChatBotService
    {

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNameCaseInsensitive = true
        };


        public async Task<int> StartNewChatAsync(string lawyerId, CreateNewChatRequest createNewChatRequest)
        {
            CheckLawyerIdentifierExistance(lawyerId);

            var chatSession = _mapper.Map<ChatSession>(createNewChatRequest);
            chatSession.CreatedAt = DateTime.UtcNow;
            chatSession.LawyerId = lawyerId;

            await _unitOfWork.GetRepository<int, ChatSession>().Add(chatSession);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new ChatSessionCreationFailedException("Failed to Create New Chat. Please try again later.");

            return chatSession.Id;
        }



        public async Task<string> SendMessageAsync(string lawyerId,
                                                   int chatSessionId,
                                                   SendMessageRequest sendMessageRequest)
        {
            CheckLawyerIdentifierExistance(lawyerId);

            await ValidateChatSessionExistanceAndLawyerAccessAsync(chatSessionId, lawyerId, false);

            await SaveMessageAsync(chatSessionId, sendMessageRequest.Content, MessageSender.User);

            var chronologicalHistory = await GetHistoryAsync(chatSessionId);

            // المسدج الجديده بقت من ضمن الهيستوري
            string aiResponseText = "Hi, Iam Rag Model"; /*await Call_RAGModel_Api_Async(chronologicalHistory);*/

            var savedAiMessage = await SaveMessageAsync(chatSessionId, aiResponseText, MessageSender.AI);

            return aiResponseText; // _mapper.Map<ChatMessagesResponse>(savedAiMessage);
        }

        private async Task<ChatSession> ValidateChatSessionExistanceAndLawyerAccessAsync(int chatSessionId, string lawyerId, bool includeMessages)
        {
            var chatSessionSpec = new ChatSessionSpecifications(chatSessionId, includeMessages);
            var chatSession = await _unitOfWork.GetRepository<int, ChatSession>().GetByIdAsync(chatSessionSpec);
            if (chatSession is null)
                throw new ChatSessionNotFoundException($"ChatSession with id : {chatSessionId} not found.");
            if (chatSession.LawyerId != lawyerId)
                throw new UnauthorizedAccessException("You don't have permission to do this action.");
            return chatSession;
        }

        private async Task<ChatMessage> SaveMessageAsync(int sessionId, string content, MessageSender sender)
        {
            var message = new ChatMessage
            {
                ChatSessionId = sessionId,
                Content = content,
                MessageSender = sender,
                CreatedAt = DateTime.UtcNow
            };
            await _unitOfWork.GetRepository<int, ChatMessage>().Add(message);
            await _unitOfWork.SaveChangesAsync();
            return message;
        }

        private async Task<List<ChatMessage>> GetHistoryAsync(int chatSessionId)
        {
            var spec = new ChatMessagesSpecification(chatSessionId);
            var history = await _unitOfWork.GetRepository<int, ChatMessage>().GetAllAsync(spec);
            var chronologicalHistory = history.Reverse().ToList();
            return chronologicalHistory;
        }


        #region Old
        //private async Task<string> Call_RAGModel_Api_Async(List<ChatMessage>? history)
        //{
        //    var client = _httpClientFactory.CreateClient("RAGModelServiceClient");

        //    //// تحويل الهيستوري لشكل يفهمه الـ AI
        //    var aiRequest = new AIRequest()
        //    {
        //        Messages = history.Select(chatMessage => new AIMessage
        //        {
        //            Role = chatMessage.MessageSender == MessageSender.User ? "user" : "assistant",
        //            Content = chatMessage.Content,
        //        }).ToList()
        //    };

        //    var response = await client.PostAsJsonAsync("ask-legal", aiRequest); // افترضنا الـ Endpoint اسمها ask-legal

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = await response.Content.ReadFromJsonAsync<AIResponse>();
        //        return result?.Answer ?? "Sorry, an error occurred while processing the AI response.";
        //    }
        //    return "The AI model is currently unavailable. Please try again later.";
        //} 
        #endregion



        private async Task<string> Call_RAGModel_Api_Async(List<ChatMessage> chatHistory)
        {
            var client = _httpClientFactory.CreateClient("RAGModelServiceClient");

            var lastUserMessage = chatHistory.LastOrDefault(message => message.MessageSender == MessageSender.User);
            if (lastUserMessage is null) return "لم يتم العثور على سؤال للمعالجة.";

            var conversationHistory = chatHistory.SkipLast(1).Select(message => new AIMessage
            {
                Role = message.MessageSender == MessageSender.User ? "user" : "assistant",
                Content = message.Content,
            }).ToList();

            var aiRequest = new AIRequest
            {
                Question = lastUserMessage.Content,
                ChatHistory = conversationHistory,
                MaxRelevantChunksCount = 5
            };

            try
            {
                var response = await client.PostAsJsonAsync($"{client.BaseAddress}query", aiRequest, _jsonOptions);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AIResponse>(_jsonOptions);
                    if (result is null) return "عذراً، حدث خطأ في معالجة الإجابة.";

                    // استخدام ميثود التنسيق لإضافة المراجع القانونية
                    return FormatAnswerWithSources(result);
                }
                _logger.LogError($"RAG API Error: {response.StatusCode}");
                return "خدمة الذكاء الاصطناعي غير متاحة حالياً.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during AI call");
                return "حدث خطأ غير متوقع أثناء الاتصال بالـ AI.";
            }
        }


        private string FormatAnswerWithSources(AIResponse result)
        {
            var answer = result.GeneratedAnswer?.Trim() ?? "";

            if (result.References == null || result.References.Count == 0)
                return answer;

            // ترتيب المراجع حسب الثقة (MatchConfidence) وإضافة أسمائهم
            var sourceLines = result.References
                .OrderByDescending(reference => reference.MatchConfidence)
                .Select((reference, i) => $"{i + 1}. {reference.CleanName}");

            var sourcesBlock = string.Join("\n", sourceLines);

            return $"{answer}\n\n---\n**المراجع القانونية:**\n{sourcesBlock}";
        }


        private void CheckLawyerIdentifierExistance(string lawyerId)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");
        }


        public async Task<IEnumerable<ChatSessionResponse>> MyChatSessionsAsync(string lawyerId, string? search = null)
        {
            CheckLawyerIdentifierExistance(lawyerId);
            var spec = new ChatSessionSpecifications(lawyerId, search);
            var allChatSessions = await _unitOfWork.GetRepository<int, ChatSession>().GetAllAsync(spec);
            var allChatSessionsResponse = _mapper.Map<IEnumerable<ChatSessionResponse>>(allChatSessions);
            return allChatSessionsResponse;
        }


        public async Task<IEnumerable<ChatMessagesResponse>> GetMessagesAsync(int chatSessionId, string lawyerId)
        {
            CheckLawyerIdentifierExistance(lawyerId);
            var chatSession = await ValidateChatSessionExistanceAndLawyerAccessAsync(chatSessionId, lawyerId, true);
            var chatMessagesResponse = _mapper.Map<IEnumerable<ChatMessagesResponse>>(chatSession.ChatMessages);
            return chatMessagesResponse;
        }

        public async Task DeleteChatAsync(int chatSessionId, string lawyerId)
        {
            CheckLawyerIdentifierExistance(lawyerId);
            var chatSession = await ValidateChatSessionExistanceAndLawyerAccessAsync(chatSessionId, lawyerId, false);
            chatSession.IsDeleted = true;
            chatSession.DeletedAt = DateTime.UtcNow;
            int deletionResult = await _unitOfWork.SaveChangesAsync();
            if (deletionResult <= 0)
                throw new ChatSessionDeletionFailedException("Failed to delete case. Please try again later.");
        }

        public async Task UpdateChatTitleAsync(int chatSessionId, string lawyerId, UpdateChatTitleRequest updateChatTitleRequest)
        {
            CheckLawyerIdentifierExistance(lawyerId);
            var chatSession = await ValidateChatSessionExistanceAndLawyerAccessAsync(chatSessionId, lawyerId, false);
            _mapper.Map(updateChatTitleRequest, chatSession);
            int updateResult = await _unitOfWork.SaveChangesAsync();
            if(updateResult <= 0)
                throw new ChatSessionUpdatedFailedException("Failed to update chat title. Please try again later.");
        }


        public async Task<ChatSessionResponseToUpdate> GetChatSessionAsync(int chatSessionId, string lawyerId)
        {
            CheckLawyerIdentifierExistance(lawyerId);
            var chatSession = await ValidateChatSessionExistanceAndLawyerAccessAsync(chatSessionId, lawyerId, false);
            return _mapper.Map<ChatSessionResponseToUpdate>(chatSession);
        }
    }




}
