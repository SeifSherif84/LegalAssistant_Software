using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot.New
{
    public class SendMessageResponse
    {
        /// <summary>Arabic answer text from Conan.</summary>
        public string AiAnswer { get; set; } = string.Empty;

        /// <summary>
        /// Float [0,1]. Display as percentage.
        /// Below 0.5 the server will already include a low-confidence warning.
        /// </summary>
        public double ConfidenceScore { get; set; }

        /// <summary>
        /// Arabic warning strings from the API.
        /// UI should render these; prefix-match against known prefixes in
        /// ConanWarningPrefixes for special treatment (red banner, info chip, etc.)
        /// </summary>
        public List<string> Warnings { get; set; } = [];

        /// <summary>Retrieved chunks the answer was grounded on.</summary>
        public List<SourceInfoResponse> Sources { get; set; } = [];

        public bool ConflictsDetected { get; set; }

        /// <summary>
        /// True when confidence_factors.article_validation == "failed".
        /// Strongest signal of an ungrounded citation — UI should show a red banner.
        /// </summary>
        public bool HasArticleValidationFailure { get; set; }

        /// <summary>
        /// True when the API returned 503 (LLM budget exhausted).
        /// UI should show a "try again later" message instead of treating this as an error.
        /// </summary>
        public bool IsServiceBusy { get; set; }
    }
}
