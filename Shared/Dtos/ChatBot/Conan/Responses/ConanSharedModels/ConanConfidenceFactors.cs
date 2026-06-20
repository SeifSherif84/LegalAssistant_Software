using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot.Conan.Responses.ConanSharedModels
{
    public record ConanConfidenceFactors(
            double RerankSignal,
            int SourceCount,
            string ArticleValidation,   // "passed" | "failed" | "not_applicable"
            bool TopicMatch);
}
