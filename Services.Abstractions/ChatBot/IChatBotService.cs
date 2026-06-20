using Microsoft.AspNetCore.Http;
using Shared.Dtos.ChatBot;
using Shared.Dtos.ChatBot.Conan;
using Shared.Dtos.ChatBot.Conan.Responses;
using Shared.Dtos.ChatBot.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Dtos.ChatBot.Conan.Requests;

namespace Services.Abstractions.ChatBot
{
    public interface IChatBotService
    {
        #region Old
        //Task<int> StartNewChatAsync(string lawyerId, CreateNewChatRequest createNewChatRequest);
        //Task<string> SendMessageAsync(string lawyerId,
        //                              int chatSessionId,
        //                              SendMessageRequest sendMessageRequest);
        //Task<IEnumerable<ChatSessionResponse>> MyChatSessionsAsync(string lawyerId, string? search);
        //Task<IEnumerable<ChatMessagesResponse>> GetMessagesAsync(int chatSessionId, string lawyerId);
        //Task DeleteChatAsync(int chatSessionId, string lawyerId);
        //Task UpdateChatTitleAsync(int chatSessionId, string lawyerId, UpdateChatTitleRequest updateChatTitleRequest);
        //Task<ChatSessionResponseToUpdate> GetChatSessionAsync(int chatSessionId, string lawyerId);
        #endregion
        Task<int> StartNewChatAsync(string lawyerId, CreateNewChatRequest createNewChatRequest);
        Task<IEnumerable<ChatSessionResponse>> MyChatSessionsAsync(string lawyerId, string? search = null);
        Task<ChatSessionResponseToUpdate> GetChatSessionAsync(int chatSessionId, string lawyerId);
        Task UpdateChatTitleAsync(int chatSessionId, string lawyerId, UpdateChatTitleRequest request);
        Task DeleteChatAsync(int chatSessionId, string lawyerId);
        Task<IEnumerable<ChatMessagesResponse>> GetMessagesAsync(int chatSessionId, string lawyerId);

        // ── Messaging ─────────────────────────────────────────────────────────────
        Task<SendMessageResponse> SendMessageAsync(string lawyerId, int chatSessionId, SendMessageRequest sendMessageRequest);

        // ── Document attachment (session-scoped) ──────────────────────────────────
        Task<ConanAttachResponse?> AttachDocumentAsync(string lawyerId, int chatSessionId, IFormFile file);
        Task<ConanAttachmentsListResponse?> GetAttachmentsAsync(string lawyerId, int chatSessionId);
        Task<bool> RemoveAttachmentAsync(string lawyerId, int chatSessionId, string docId);
        Task<bool> ClearAttachmentsAsync(string lawyerId, int chatSessionId);
        Task<ConanApiResult<ConanAnswerEnvelope>> AnalyzeWeaknessAndSaveAsync(string lawyerId,int chatSessionId,IFormFile file,string? evidence = null,string? defendantStatement = null,CancellationToken ct = default);
        Task<ConanApiResult<ConanAnswerEnvelope>> GenerateDefenseMemoAndSaveAsync(string lawyerId, int chatSessionId, IFormFile file, string? weaknesses = null, string? evidence = null, string? defendantStatement = null, CancellationToken ct = default);
        Task<ConanApiResult<ConanSummarizeResponse>> SummarizeFromFileAsyncAndSaveAsync( string lawyerId, int chatSessionId, IFormFile file, CancellationToken ct = default);

    }
}
