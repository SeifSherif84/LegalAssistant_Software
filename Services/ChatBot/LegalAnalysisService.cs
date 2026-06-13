using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Services.Abstractions.ChatBot;
using Shared.Dtos.ChatBot.Conan;
using Shared.Dtos.ChatBot.Conan.Responses;
using static Shared.Dtos.ChatBot.Conan.Requests;

namespace Services.ChatBot 
{
    public class LegalAnalysisService(
        IConanApiService _conanApi,
        ILogger<LegalAnalysisService> _logger) : ILegalAnalysisService
    {
        // ── Health ────────────────────────────────────────────────────────────────

        public Task<ConanHealthResponse?> GetHealthAsync(CancellationToken ct = default)
            => _conanApi.GetHealthAsync(ct);


        // ── Parse ─────────────────────────────────────────────────────────────────

        public Task<ConanApiResult<ConanParseResponse>> ParseFileAsync(
            IFormFile file, CancellationToken ct = default)
        {
            if (!ValidateFile(file, out var error))
                return Task.FromResult(ConanApiResult<ConanParseResponse>.BadRequest(error));

            return _conanApi.ParseDocumentAsync(file, ct);
        }

        public Task<ConanApiResult<ConanParseResponse>> ParseTextAsync(
            string text, CancellationToken ct = default)
            => _conanApi.ParseTextAsync(text, ct);


        // ── Q&A ───────────────────────────────────────────────────────────────────

        public Task<ConanApiResult<ConanAnswerEnvelope>> AskQuestionAsync(
            ConanQaRequest request, CancellationToken ct = default)
            => _conanApi.AskQuestionAsync(request, ct);

        public Task<ConanApiResult<ConanAnswerEnvelope>> AskQuestionWithFileAsync(
            string question, IFormFile file, int k = 10, CancellationToken ct = default)
        {
            if (!ValidateFile(file, out var error))
                return Task.FromResult(ConanApiResult<ConanAnswerEnvelope>.BadRequest(error));

            return _conanApi.AskQuestionWithFileAsync(question, file, k, ct);
        }


        // ── Weakness ──────────────────────────────────────────────────────────────

        public Task<ConanApiResult<ConanAnswerEnvelope>> AnalyzeWeaknessAsync(
            ConanWeaknessRequest request, CancellationToken ct = default)
            => _conanApi.AnalyzeWeaknessAsync(request, ct);

        public async Task<ConanApiResult<ConanAnswerEnvelope>> AnalyzeWeaknessFromFileAsync(
            IFormFile file,
            string? evidence = null,
            string? defendantStatement = null,
            CancellationToken ct = default)
        {
            // 1. Parse
            var parseResult = await ParseAndExtractAsync<ConanAnswerEnvelope>(file, ct);
            if (parseResult.IsFailed) return parseResult.FailureResult!;

            // 2. Analyse
            return await _conanApi.AnalyzeWeaknessAsync(
                new ConanWeaknessRequest(parseResult.Text!, evidence, defendantStatement), ct);
        }


        // ── Defense ───────────────────────────────────────────────────────────────

        public Task<ConanApiResult<ConanAnswerEnvelope>> GenerateDefenseMemoAsync(
            ConanDefenseRequest request, CancellationToken ct = default)
            => _conanApi.GenerateDefenseMemoAsync(request, ct);

        public async Task<ConanApiResult<ConanAnswerEnvelope>> GenerateDefenseMemoFromFileAsync(
            IFormFile file,
            string? weaknesses = null,
            string? evidence = null,
            string? defendantStatement = null,
            CancellationToken ct = default)
        {
            // 1. Parse
            var parseResult = await ParseAndExtractAsync<ConanAnswerEnvelope>(file, ct);
            if (parseResult.IsFailed) return parseResult.FailureResult!;

            // 2. Generate memo
            return await _conanApi.GenerateDefenseMemoAsync(
                new ConanDefenseRequest(parseResult.Text!, weaknesses, evidence, defendantStatement), ct);
        }


        // ── Forensic ──────────────────────────────────────────────────────────────

        public Task<ConanApiResult<ConanAnswerEnvelope>> AnalyzeForensicAsync(
            ConanForensicRequest request, CancellationToken ct = default)
            => _conanApi.AnalyzeForensicConsistencyAsync(request, ct);

        public async Task<ConanApiResult<ConanAnswerEnvelope>> AnalyzeForensicFromFileAsync(
            IFormFile file,
            string? evidence = null,
            CancellationToken ct = default)
        {
            // 1. Parse
            var parseResult = await ParseAndExtractAsync<ConanAnswerEnvelope>(file, ct);
            if (parseResult.IsFailed) return parseResult.FailureResult!;

            // 2. Analyse
            return await _conanApi.AnalyzeForensicConsistencyAsync(
                new ConanForensicRequest(parseResult.Text!, evidence), ct);
        }


        // ── Summarize ─────────────────────────────────────────────────────────────

        public Task<ConanApiResult<ConanSummarizeResponse>> SummarizeAsync(
            ConanSummarizeRequest request, CancellationToken ct = default)
            => _conanApi.SummarizeTextAsync(request, ct);

        public async Task<ConanApiResult<ConanSummarizeResponse>> SummarizeFromFileAsync(
            IFormFile file, CancellationToken ct = default)
        {
            // 1. Parse
            var parseResult = await ParseAndExtractAsync<ConanSummarizeResponse>(file, ct);
            if (parseResult.IsFailed) return parseResult.FailureResult!;

            // 2. Summarize
            return await _conanApi.SummarizeTextAsync(
                new ConanSummarizeRequest(parseResult.Text!), ct);
        }


        // ── Private helpers ───────────────────────────────────────────────────────

        private async Task<ParseExtractResult<T>> ParseAndExtractAsync<T>(
            IFormFile file, CancellationToken ct)
        {
            // Validate first — no network call needed
            if (!ValidateFile(file, out var validationError))
                return ParseExtractResult<T>.Fail(
                    ConanApiResult<T>.BadRequest(validationError));

            // Call /parse
            var parseResult = await _conanApi.ParseDocumentAsync(file, ct);

            if (!parseResult.IsSuccess || parseResult.Data is null)
            {
                _logger.LogWarning("[LegalAnalysis] /parse failed for {FileName}: {Detail}",
                    file.FileName, parseResult.ErrorDetail);

                return ParseExtractResult<T>.Fail(
                    ConanApiResult<T>.BadRequest(
                        parseResult.ErrorDetail ?? "فشل استخراج النص من الملف."));
            }

            return ParseExtractResult<T>.Ok(parseResult.Data.Text);
        }

        private static bool ValidateFile(IFormFile? file, out string error)
        {
            if (file is null || file.Length == 0)
            {
                error = "يرجى إرفاق ملف القضية.";
                return false;
            }

            if (Path.GetExtension(file.FileName).ToLowerInvariant() == ".doc")
            {
                error = "صيغة .doc غير مدعومة. يرجى تحويل الملف إلى .docx أو .pdf أو .txt";
                return false;
            }

            error = string.Empty;
            return true;
        }


        // ── Value object ─────────────────────────────────────────────────────────

        private readonly record struct ParseExtractResult<T>
        {
            public string? Text { get; init; }
            public ConanApiResult<T>? FailureResult { get; init; }
            public bool IsFailed => FailureResult is not null;

            public static ParseExtractResult<T> Ok(string text)
                => new() { Text = text };

            public static ParseExtractResult<T> Fail(ConanApiResult<T> result)
                => new() { FailureResult = result };
        }
    }
}

