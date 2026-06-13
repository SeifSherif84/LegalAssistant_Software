using Shared.Dtos.ChatBot.Conan.Responses.ConanSharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot.Conan.Responses
{
    public class ConanAnswerEnvelope
    {
        // ── main text fields (only one is non-null per endpoint) ──
        public string? Answer { get; init; }
        public string? Analysis { get; init; }
        public string? Memorandum { get; init; }

        // ── confidence ──
        public double ConfidenceScore { get; init; }
        public ConanConfidenceFactors? ConfidenceFactors { get; init; }

        // ── sources & warnings ──
        public List<ConanSourceInfo> Sources { get; init; } = [];
        public List<string> Warnings { get; init; } = [];
        public bool ConflictsDetected { get; init; }

        // ── timing & model (DYNAMIC — never branch on model value) ──
        public double LatencyMs { get; init; }
        public double? RetrievalMs { get; init; }
        public string Model { get; init; } = string.Empty;

        // ── chat-only extras ──
        public string? SessionId { get; init; }
        public int? TurnCount { get; init; }
        public bool? WasCompacted { get; init; }

        // ── defense-only extra ──
        public int? SelfCheckRevisions { get; init; }

        public string GetMainText() => Answer ?? Analysis ?? Memorandum ?? string.Empty;

        public bool HasArticleValidationFailure =>
            ConfidenceFactors?.ArticleValidation == "failed";
    }
}
