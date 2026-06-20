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
    public interface ILegalAnalysisService
    {
        // ── Health ────────────────────────────────────────────────────────────────
        Task<ConanHealthResponse?> GetHealthAsync(CancellationToken ct = default);

        // ── Parse ─────────────────────────────────────────────────────────────────
        Task<ConanApiResult<ConanParseResponse>> ParseFileAsync(IFormFile file, CancellationToken ct = default);
        Task<ConanApiResult<ConanParseResponse>> ParseTextAsync(string text, CancellationToken ct = default);

        // ── Q&A ───────────────────────────────────────────────────────────────────
        Task<ConanApiResult<ConanAnswerEnvelope>> AskQuestionAsync(ConanQaRequest request, CancellationToken ct = default);
        Task<ConanApiResult<ConanAnswerEnvelope>> AskQuestionWithFileAsync(string question, IFormFile file, int k = 10, CancellationToken ct = default);

        // ── Weakness ──────────────────────────────────────────────────────────────
        Task<ConanApiResult<ConanAnswerEnvelope>> AnalyzeWeaknessAsync(ConanWeaknessRequest request, CancellationToken ct = default);
        Task<ConanApiResult<ConanAnswerEnvelope>> AnalyzeWeaknessFromFileAsync(
            IFormFile file,
            string? evidence = null,
            string? defendantStatement = null,
            CancellationToken ct = default);

        // ── Defense ───────────────────────────────────────────────────────────────
        Task<ConanApiResult<ConanAnswerEnvelope>> GenerateDefenseMemoAsync(ConanDefenseRequest request, CancellationToken ct = default);
        Task<ConanApiResult<ConanAnswerEnvelope>> GenerateDefenseMemoFromFileAsync(
            IFormFile file,
            string? weaknesses = null,
            string? evidence = null,
            string? defendantStatement = null,
            CancellationToken ct = default);

        // ── Forensic ──────────────────────────────────────────────────────────────
        Task<ConanApiResult<ConanAnswerEnvelope>> AnalyzeForensicAsync(ConanForensicRequest request, CancellationToken ct = default);
        Task<ConanApiResult<ConanAnswerEnvelope>> AnalyzeForensicFromFileAsync(
            IFormFile file,
            string? evidence = null,
            CancellationToken ct = default);

        // ── Summarize ─────────────────────────────────────────────────────────────
        Task<ConanApiResult<ConanSummarizeResponse>> SummarizeAsync(ConanSummarizeRequest request, CancellationToken ct = default);
        Task<ConanApiResult<ConanSummarizeResponse>> SummarizeFromFileAsync(IFormFile file, CancellationToken ct = default);
    }
}
