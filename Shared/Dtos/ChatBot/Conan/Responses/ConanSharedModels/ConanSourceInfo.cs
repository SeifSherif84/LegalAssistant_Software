using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot.Conan.Responses.ConanSharedModels
{
    public record ConanSourceInfo(
            string Filename,
            string? Source,
            string? DocType,
            string? LegalCategory,
            string? LegalTopic,
            string? Article,
            List<string> ReferencedArticles,
            int? Page,
            double RetrievalScore,
            double RerankScore);
}
