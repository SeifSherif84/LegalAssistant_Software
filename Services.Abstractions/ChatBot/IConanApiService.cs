using Microsoft.AspNetCore.Http;
using Shared.Dtos.ChatBot.Conan.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Dtos.ChatBot.Conan.Requests;

namespace Services.Abstractions.ChatBot
{
    public interface IConanApiService
    {
        Task<ConanHealthResponse?> GetHealthAsync(CancellationToken ct = default);

        // ── Q&A ─────────────────────────────────────────────────────────────────
        Task<ConanApiResult<ConanAnswerEnvelope>> AskQuestionAsync(
            ConanQaRequest request,
            CancellationToken ct = default);

        Task<ConanApiResult<ConanAnswerEnvelope>> AskQuestionWithFileAsync(
            string question,
            IFormFile file,
            int k = 7,
            CancellationToken ct = default);

        // ── Multi-turn chat ─────────────────────────────────────────────────────
        Task<ConanApiResult<ConanAnswerEnvelope>> SendChatMessageAsync(
            ConanChatRequest request,
            CancellationToken ct = default);

        // Attach a document to a session so it rides along on every subsequent turn.
        Task<ConanApiResult<ConanAttachResponse>> AttachDocumentToSessionAsync(
            string sessionId,
            IFormFile file,
            CancellationToken ct = default);

        Task<ConanAttachmentsListResponse?> GetSessionAttachmentsAsync(
            string sessionId,
            CancellationToken ct = default);

        Task<bool> RemoveSessionAttachmentAsync(
            string sessionId,
            string docId,
            CancellationToken ct = default);

        Task<bool> ClearSessionAttachmentsAsync(
            string sessionId,
            CancellationToken ct = default);

        // ── Legal analysis endpoints ────────────────────────────────────────────
        Task<ConanApiResult<ConanAnswerEnvelope>> AnalyzeWeaknessAsync(
            ConanWeaknessRequest request,
            CancellationToken ct = default);

        Task<ConanApiResult<ConanAnswerEnvelope>> GenerateDefenseMemoAsync(
            ConanDefenseRequest request,
            CancellationToken ct = default);

        Task<ConanApiResult<ConanAnswerEnvelope>> AnalyzeForensicConsistencyAsync(
            ConanForensicRequest request,
            CancellationToken ct = default);

        // ── Utilities ───────────────────────────────────────────────────────────
        Task<ConanApiResult<ConanSummarizeResponse>> SummarizeTextAsync(
            ConanSummarizeRequest request,
            CancellationToken ct = default);

        Task<ConanApiResult<ConanParseResponse>> ParseDocumentAsync(
            IFormFile file,
            CancellationToken ct = default);

        Task<ConanApiResult<ConanParseResponse>> ParseTextAsync(
            string text,
            CancellationToken ct = default);
    }
}
