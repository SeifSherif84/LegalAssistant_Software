#region Old2
//using AutoMapper;
//using Domain.Contracts;
//using Domain.Entities;
//using Domain.Entities.ChatBotAIEntities;
//using Domain.Entities.ChatBotAIEntities.Enums;
//using Domain.Exceptions.BadRequest;
//using Domain.Exceptions.NotFound;
//using Microsoft.AspNetCore.Mvc;
//using Org.BouncyCastle.Asn1.Ocsp;
//using Services.Abstractions.ChatBot;
//using Services.Specifications.ChatBot;
//using Shared.Dtos.ChatBot;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http.Json;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using Shared.Dtos.ChatBot.Enums;
//using Microsoft.AspNetCore.Http.Json;
//using System.Text.Encodings.Web;
//using System.Text.Json;
//using Microsoft.Extensions.Logging;

//namespace Services.ChatBot
//{
//    public class ChatBotService(IUnitOfWork _unitOfWork,
//                                IMapper _mapper,
//                                IHttpClientFactory _httpClientFactory,
//                                ILogger<ChatBotService> _logger) : IChatBotService
//    {

//        private static readonly JsonSerializerOptions _jsonOptions = new()
//        {
//            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
//            PropertyNameCaseInsensitive = true
//        };


//        public async Task<int> StartNewChatAsync(string lawyerId, CreateNewChatRequest createNewChatRequest)
//        {
//            CheckLawyerIdentifierExistance(lawyerId);

//            var chatSession = _mapper.Map<ChatSession>(createNewChatRequest);
//            chatSession.CreatedAt = DateTime.UtcNow;
//            chatSession.LawyerId = lawyerId;

//            await _unitOfWork.GetRepository<int, ChatSession>().Add(chatSession);
//            var result = await _unitOfWork.SaveChangesAsync();
//            if (result <= 0)
//                throw new ChatSessionCreationFailedException("Failed to Create New Chat. Please try again later.");

//            return chatSession.Id;
//        }



//        public async Task<string> SendMessageAsync(string lawyerId,
//                                                   int chatSessionId,
//                                                   SendMessageRequest sendMessageRequest)
//        {
//            CheckLawyerIdentifierExistance(lawyerId);

//            await ValidateChatSessionExistanceAndLawyerAccessAsync(chatSessionId, lawyerId, false);

//            await SaveMessageAsync(chatSessionId, sendMessageRequest.Content, MessageSender.User);

//            var chronologicalHistory = await GetHistoryAsync(chatSessionId);

//            // المسدج الجديده بقت من ضمن الهيستوري
//            string aiResponseText = "Hi, Iam Rag Model"; /*await Call_RAGModel_Api_Async(chronologicalHistory);*/

//            var savedAiMessage = await SaveMessageAsync(chatSessionId, aiResponseText, MessageSender.AI);

//            return aiResponseText; // _mapper.Map<ChatMessagesResponse>(savedAiMessage);
//        }

//        private async Task<ChatSession> ValidateChatSessionExistanceAndLawyerAccessAsync(int chatSessionId, string lawyerId, bool includeMessages)
//        {
//            var chatSessionSpec = new ChatSessionSpecifications(chatSessionId, includeMessages);
//            var chatSession = await _unitOfWork.GetRepository<int, ChatSession>().GetByIdAsync(chatSessionSpec);
//            if (chatSession is null)
//                throw new ChatSessionNotFoundException($"ChatSession with id : {chatSessionId} not found.");
//            if (chatSession.LawyerId != lawyerId)
//                throw new UnauthorizedAccessException("You don't have permission to do this action.");
//            return chatSession;
//        }

//        private async Task<ChatMessage> SaveMessageAsync(int sessionId, string content, MessageSender sender)
//        {
//            var message = new ChatMessage
//            {
//                ChatSessionId = sessionId,
//                Content = content,
//                MessageSender = sender,
//                CreatedAt = DateTime.UtcNow
//            };
//            await _unitOfWork.GetRepository<int, ChatMessage>().Add(message);
//            await _unitOfWork.SaveChangesAsync();
//            return message;
//        }

//        private async Task<List<ChatMessage>> GetHistoryAsync(int chatSessionId)
//        {
//            var spec = new ChatMessagesSpecification(chatSessionId);
//            var history = await _unitOfWork.GetRepository<int, ChatMessage>().GetAllAsync(spec);
//            var chronologicalHistory = history.Reverse().ToList();
//            return chronologicalHistory;
//        }


//        #region Old
//        //private async Task<string> Call_RAGModel_Api_Async(List<ChatMessage>? history)
//        //{
//        //    var client = _httpClientFactory.CreateClient("RAGModelServiceClient");

//        //    //// تحويل الهيستوري لشكل يفهمه الـ AI
//        //    var aiRequest = new AIRequest()
//        //    {
//        //        Messages = history.Select(chatMessage => new AIMessage
//        //        {
//        //            Role = chatMessage.MessageSender == MessageSender.User ? "user" : "assistant",
//        //            Content = chatMessage.Content,
//        //        }).ToList()
//        //    };

//        //    var response = await client.PostAsJsonAsync("ask-legal", aiRequest); // افترضنا الـ Endpoint اسمها ask-legal

//        //    if (response.IsSuccessStatusCode)
//        //    {
//        //        var result = await response.Content.ReadFromJsonAsync<AIResponse>();
//        //        return result?.Answer ?? "Sorry, an error occurred while processing the AI response.";
//        //    }
//        //    return "The AI model is currently unavailable. Please try again later.";
//        //} 
//        #endregion



//        private async Task<string> Call_RAGModel_Api_Async(List<ChatMessage> chatHistory)
//        {
//            var client = _httpClientFactory.CreateClient("RAGModelServiceClient");

//            var lastUserMessage = chatHistory.LastOrDefault(message => message.MessageSender == MessageSender.User);
//            if (lastUserMessage is null) return "لم يتم العثور على سؤال للمعالجة.";

//            var conversationHistory = chatHistory.SkipLast(1).Select(message => new AIMessage
//            {
//                Role = message.MessageSender == MessageSender.User ? "user" : "assistant",
//                Content = message.Content,
//            }).ToList();

//            var aiRequest = new AIRequest
//            {
//                Question = lastUserMessage.Content,
//                ChatHistory = conversationHistory,
//                MaxRelevantChunksCount = 5
//            };

//            try
//            {
//                var response = await client.PostAsJsonAsync($"{client.BaseAddress}query", aiRequest, _jsonOptions);
//                if (response.IsSuccessStatusCode)
//                {
//                    var result = await response.Content.ReadFromJsonAsync<AIResponse>(_jsonOptions);
//                    if (result is null) return "عذراً، حدث خطأ في معالجة الإجابة.";

//                    // استخدام ميثود التنسيق لإضافة المراجع القانونية
//                    return FormatAnswerWithSources(result);
//                }
//                _logger.LogError($"RAG API Error: {response.StatusCode}");
//                return "خدمة الذكاء الاصطناعي غير متاحة حالياً.";
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Unexpected error during AI call");
//                return "حدث خطأ غير متوقع أثناء الاتصال بالـ AI.";
//            }
//        }


//        private string FormatAnswerWithSources(AIResponse result)
//        {
//            var answer = result.GeneratedAnswer?.Trim() ?? "";

//            if (result.References == null || result.References.Count == 0)
//                return answer;

//            // ترتيب المراجع حسب الثقة (MatchConfidence) وإضافة أسمائهم
//            var sourceLines = result.References
//                .OrderByDescending(reference => reference.MatchConfidence)
//                .Select((reference, i) => $"{i + 1}. {reference.CleanName}");

//            var sourcesBlock = string.Join("\n", sourceLines);

//            return $"{answer}\n\n---\n**المراجع القانونية:**\n{sourcesBlock}";
//        }


//        private void CheckLawyerIdentifierExistance(string lawyerId)
//        {
//            // 1. Check LawyerId
//            if (string.IsNullOrEmpty(lawyerId))
//                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");
//        }


//        public async Task<IEnumerable<ChatSessionResponse>> MyChatSessionsAsync(string lawyerId, string? search = null)
//        {
//            CheckLawyerIdentifierExistance(lawyerId);
//            var spec = new ChatSessionSpecifications(lawyerId, search);
//            var allChatSessions = await _unitOfWork.GetRepository<int, ChatSession>().GetAllAsync(spec);
//            var allChatSessionsResponse = _mapper.Map<IEnumerable<ChatSessionResponse>>(allChatSessions);
//            return allChatSessionsResponse;
//        }


//        public async Task<IEnumerable<ChatMessagesResponse>> GetMessagesAsync(int chatSessionId, string lawyerId)
//        {
//            CheckLawyerIdentifierExistance(lawyerId);
//            var chatSession = await ValidateChatSessionExistanceAndLawyerAccessAsync(chatSessionId, lawyerId, true);
//            var chatMessagesResponse = _mapper.Map<IEnumerable<ChatMessagesResponse>>(chatSession.ChatMessages);
//            return chatMessagesResponse;
//        }

//        public async Task DeleteChatAsync(int chatSessionId, string lawyerId)
//        {
//            CheckLawyerIdentifierExistance(lawyerId);
//            var chatSession = await ValidateChatSessionExistanceAndLawyerAccessAsync(chatSessionId, lawyerId, false);
//            chatSession.IsDeleted = true;
//            chatSession.DeletedAt = DateTime.UtcNow;
//            int deletionResult = await _unitOfWork.SaveChangesAsync();
//            if (deletionResult <= 0)
//                throw new ChatSessionDeletionFailedException("Failed to delete case. Please try again later.");
//        }

//        public async Task UpdateChatTitleAsync(int chatSessionId, string lawyerId, UpdateChatTitleRequest updateChatTitleRequest)
//        {
//            CheckLawyerIdentifierExistance(lawyerId);
//            var chatSession = await ValidateChatSessionExistanceAndLawyerAccessAsync(chatSessionId, lawyerId, false);
//            _mapper.Map(updateChatTitleRequest, chatSession);
//            int updateResult = await _unitOfWork.SaveChangesAsync();
//            if(updateResult <= 0)
//                throw new ChatSessionUpdatedFailedException("Failed to update chat title. Please try again later.");
//        }


//        public async Task<ChatSessionResponseToUpdate> GetChatSessionAsync(int chatSessionId, string lawyerId)
//        {
//            CheckLawyerIdentifierExistance(lawyerId);
//            var chatSession = await ValidateChatSessionExistanceAndLawyerAccessAsync(chatSessionId, lawyerId, false);
//            return _mapper.Map<ChatSessionResponseToUpdate>(chatSession);
//        }
//    }




//}
#endregion
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.ChatBotAIEntities;
using Domain.Entities.ChatBotAIEntities.Enums;
using Domain.Exceptions.BadRequest;
using Domain.Exceptions.NotFound;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Abstractions.ChatBot;
using Services.Specifications.ChatBot;
using Shared.Dtos.ChatBot;
using Shared.Dtos.ChatBot.New;
using Shared.Dtos.ChatBot.Conan;
using static Shared.Dtos.ChatBot.Conan.Requests;
using Shared.Dtos.ChatBot.Conan.Responses;

namespace Services.ChatBot;
public class ChatBotService(
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    IConanApiService _conanApi,
    ILegalAnalysisService _legalService,
    ILogger<ChatBotService> _logger) : IChatBotService
{

    // ── Session management ────────────────────────────────────────────────────

    public async Task<int> StartNewChatAsync(string lawyerId, CreateNewChatRequest createNewChatRequest)
    {
        CheckLawyerIdentifier(lawyerId);

        var chatSession = _mapper.Map<ChatSession>(createNewChatRequest);
        chatSession.CreatedAt = DateTime.UtcNow;
        chatSession.LawyerId = lawyerId;
        // ConanSessionId starts null; set on first SendMessageAsync call.

        await _unitOfWork.GetRepository<int, ChatSession>().Add(chatSession);
        var result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new ChatSessionCreationFailedException("Failed to create new chat. Please try again later.");

        await SaveMessageAsync(chatSession.Id, 
            "أهلاً، أنا كونان — مساعدك القانوني في القانون الجنائي المصري. كيف يمكنني مساعدتك اليوم؟", MessageSender.AI);

        return chatSession.Id;
    }

    public async Task<IEnumerable<ChatSessionResponse>> MyChatSessionsAsync(string lawyerId, string? search = null)
    {
        CheckLawyerIdentifier(lawyerId);
        var spec = new ChatSessionSpecifications(lawyerId, search);
        var sessions = await _unitOfWork.GetRepository<int, ChatSession>().GetAllAsync(spec);
        return _mapper.Map<IEnumerable<ChatSessionResponse>>(sessions);
    }

    public async Task<ChatSessionResponseToUpdate> GetChatSessionAsync(int chatSessionId, string lawyerId)
    {
        CheckLawyerIdentifier(lawyerId);
        var session = await ValidateSessionAccessAsync(chatSessionId, lawyerId, includeMessages: false);
        return _mapper.Map<ChatSessionResponseToUpdate>(session);
    }

    public async Task UpdateChatTitleAsync(int chatSessionId, string lawyerId, UpdateChatTitleRequest request)
    {
        CheckLawyerIdentifier(lawyerId);
        var session = await ValidateSessionAccessAsync(chatSessionId, lawyerId, includeMessages: false);
        _mapper.Map(request, session);
        int result = await _unitOfWork.SaveChangesAsync();
        if (result <= 0)
            throw new ChatSessionUpdatedFailedException("Failed to update chat title. Please try again later.");
    }

    public async Task DeleteChatAsync(int chatSessionId, string lawyerId)
    {
        CheckLawyerIdentifier(lawyerId);
        var session = await ValidateSessionAccessAsync(chatSessionId, lawyerId, includeMessages: true);

        foreach (var message in session.ChatMessages)
            _unitOfWork.GetRepository<int, ChatMessage>().Delete(message);

        session.IsDeleted = true;
        session.DeletedAt = DateTime.UtcNow;
        if (await _unitOfWork.SaveChangesAsync() <= 0)
            throw new ChatSessionDeletionFailedException("Failed to delete chat. Please try again later.");
    }

    public async Task<IEnumerable<ChatMessagesResponse>> GetMessagesAsync(int chatSessionId, string lawyerId)
    {
        CheckLawyerIdentifier(lawyerId);
        var session = await ValidateSessionAccessAsync(chatSessionId, lawyerId, includeMessages: true);
        return _mapper.Map<IEnumerable<ChatMessagesResponse>>(session.ChatMessages);
    }


    // ── Messaging ─────────────────────────────────────────────────────────────
    public async Task<SendMessageResponse> SendMessageAsync(
        string lawyerId,
        int chatSessionId,
        SendMessageRequest sendMessageRequest)
    {
        CheckLawyerIdentifier(lawyerId);
        var session = await ValidateSessionAccessAsync(chatSessionId, lawyerId, includeMessages: false);

        // 1. Persist user message
        await SaveMessageAsync(chatSessionId, sendMessageRequest.Content, MessageSender.User);

        // 2. Call Conan — echo the persisted session UUID (null on first turn)
        var conanRequest = new ConanChatRequest(
            Message: sendMessageRequest.Content,
            SessionId: session.ConanSessionId);   // null on first turn → server mints a UUID

        var apiResult = await _conanApi.SendChatMessageAsync(conanRequest);

        // 3. Handle API result
        string aiText;

        if (apiResult.IsSuccess && apiResult.Data is not null)
        {
            var envelope = apiResult.Data;
            aiText = envelope.GetMainText();

            // Persist the Conan session UUID if this was the first turn
            if (string.IsNullOrEmpty(session.ConanSessionId) && !string.IsNullOrEmpty(envelope.SessionId))
            {
                session.ConanSessionId = envelope.SessionId;
                await _unitOfWork.SaveChangesAsync();
            }

            // 4. Persist AI message
            await SaveMessageAsync(chatSessionId, aiText, MessageSender.AI);

            return new SendMessageResponse
            {
                AiAnswer = aiText,
                ConfidenceScore = envelope.ConfidenceScore,
                Warnings = envelope.Warnings,
                Sources = _mapper.Map<List<SourceInfoResponse>>(envelope.Sources),
                ConflictsDetected = envelope.ConflictsDetected,
                HasArticleValidationFailure = envelope.HasArticleValidationFailure,
                IsServiceBusy = false
            };
        }

        // 5. Graceful degradation
        if (apiResult.Status == ConanApiResultStatus.ServiceBusy)
        {
            // 503: LLM quota — do NOT crash; let the user retry
            _logger.LogWarning("[ChatBot] Conan API busy on session {SessionId}", chatSessionId);
            var busyMsg = apiResult.ErrorDetail ?? "خدمة الذكاء الاصطناعي مشغولة حالياً. يرجى المحاولة مرة أخرى.";
            await SaveMessageAsync(chatSessionId, busyMsg, MessageSender.AI);
            return new SendMessageResponse { AiAnswer = busyMsg, IsServiceBusy = true };
        }

        // Other errors
        var fallbackMsg = "حدث خطأ أثناء الاتصال بالذكاء الاصطناعي. يرجى المحاولة لاحقاً.";
        await SaveMessageAsync(chatSessionId, fallbackMsg, MessageSender.AI);
        return new SendMessageResponse { AiAnswer = fallbackMsg, IsServiceBusy = false };
    }


    // ── Document attachment (session-scoped) ──────────────────────────────────

    public async Task<ConanAttachResponse?> AttachDocumentAsync(
        string lawyerId,
        int chatSessionId,
        IFormFile file)
    {
        CheckLawyerIdentifier(lawyerId);
        var session = await ValidateSessionAccessAsync(chatSessionId, lawyerId, includeMessages: false);

        if (string.IsNullOrEmpty(session.ConanSessionId))
            throw new InvalidOperationException("Send at least one message before attaching a document.");

        var result = await _conanApi.AttachDocumentToSessionAsync(session.ConanSessionId, file);
        return result.IsSuccess ? result.Data : null;
    }

    public async Task<ConanAttachmentsListResponse?> GetAttachmentsAsync(
        string lawyerId,
        int chatSessionId)
    {
        CheckLawyerIdentifier(lawyerId);
        var session = await ValidateSessionAccessAsync(chatSessionId, lawyerId, includeMessages: false);
        if (string.IsNullOrEmpty(session.ConanSessionId)) return null;
        return await _conanApi.GetSessionAttachmentsAsync(session.ConanSessionId);
    }

    public async Task<bool> RemoveAttachmentAsync(
        string lawyerId,
        int chatSessionId,
        string docId)
    {
        CheckLawyerIdentifier(lawyerId);
        var session = await ValidateSessionAccessAsync(chatSessionId, lawyerId, includeMessages: false);
        if (string.IsNullOrEmpty(session.ConanSessionId)) return false;
        return await _conanApi.RemoveSessionAttachmentAsync(session.ConanSessionId, docId);
    }

    public async Task<bool> ClearAttachmentsAsync(string lawyerId, int chatSessionId)
    {
        CheckLawyerIdentifier(lawyerId);
        var session = await ValidateSessionAccessAsync(chatSessionId, lawyerId, includeMessages: false);
        if (string.IsNullOrEmpty(session.ConanSessionId)) return false;
        return await _conanApi.ClearSessionAttachmentsAsync(session.ConanSessionId);
    }

    // ── Weakness ───────────────────────────────────────────────────────
    public async Task<ConanApiResult<ConanAnswerEnvelope>> AnalyzeWeaknessAndSaveAsync(
        string lawyerId,
        int chatSessionId,
        IFormFile file,
        string? evidence = null,
        string? defendantStatement = null,
        CancellationToken ct = default)
    {
        CheckLawyerIdentifier(lawyerId);
        await ValidateSessionAccessAsync(chatSessionId, lawyerId, includeMessages: false);

        // 1. Call LegalAnalysisService
        var result = await _legalService.AnalyzeWeaknessFromFileAsync(
            file, evidence, defendantStatement, ct);

        // 2. Save في الـ DB
        if (result.IsSuccess && result.Data is not null)
            await SaveLegalResultAsync(chatSessionId, file.FileName, result.Data.GetMainText());

        return result;
    }
    //── Defence ───────────────────────────────────────────────────────
    public async Task<ConanApiResult<ConanAnswerEnvelope>> GenerateDefenseMemoAndSaveAsync(
        string lawyerId,
        int chatSessionId,
        IFormFile file,
        string? weaknesses = null,
        string? evidence = null,
        string? defendantStatement = null,
        CancellationToken ct = default)
    {
        CheckLawyerIdentifier(lawyerId);
        await ValidateSessionAccessAsync(chatSessionId, lawyerId, includeMessages: false);

        // 1. Call LegalAnalysisService
        var result = await _legalService.GenerateDefenseMemoFromFileAsync(
            file,weaknesses,evidence,defendantStatement,ct);

        // 2. Save في الـ DB
        if (result.IsSuccess && result.Data is not null)
            await SaveLegalResultAsync(chatSessionId, file.FileName, result.Data.GetMainText());

        return result;
    }
    //── summrization ───────────────────────────────────────────────────────

    public async Task<ConanApiResult<ConanSummarizeResponse>> SummarizeFromFileAsyncAndSaveAsync(
    string lawyerId,
    int chatSessionId,
    IFormFile file,
    CancellationToken ct = default)
    {
        CheckLawyerIdentifier(lawyerId);
        await ValidateSessionAccessAsync(chatSessionId, lawyerId, includeMessages: false);

        // 1. Call LegalAnalysisService
        var result = await _legalService.SummarizeFromFileAsync(
            file, ct);

        // 2. Save في الـ DB
        if (result.IsSuccess && result.Data is not null)
            await SaveLegalResultAsync(chatSessionId, file.FileName, result.Data.Summary);

        return result;
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private async Task<ChatSession> ValidateSessionAccessAsync(
        int chatSessionId,
        string lawyerId,
        bool includeMessages)
    {
        var spec = new ChatSessionSpecifications(chatSessionId, includeMessages);
        var session = await _unitOfWork.GetRepository<int, ChatSession>().GetByIdAsync(spec);

        if (session is null)
            throw new ChatSessionNotFoundException($"ChatSession {chatSessionId} not found.");
        if (session.LawyerId != lawyerId)
            throw new UnauthorizedAccessException("You don't have permission to access this session.");

        return session;
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

    private static void CheckLawyerIdentifier(string lawyerId)
    {
        if (string.IsNullOrEmpty(lawyerId))
            throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");
    }
    private async Task SaveLegalResultAsync(int chatSessionId, string fileName, string aiResponse)
    {
        await SaveMessageAsync(chatSessionId, $"[ملف: {fileName}]", MessageSender.User);
        await SaveMessageAsync(chatSessionId, aiResponse, MessageSender.AI);
    }
    
}