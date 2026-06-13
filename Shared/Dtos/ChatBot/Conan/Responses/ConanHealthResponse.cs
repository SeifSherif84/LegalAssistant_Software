using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot.Conan.Responses
{
    public record ConanHealthResponse(
        string Status,          // "ok" | "degraded" | "loading"
        int Vectors,
        int Chunks,
        string Model,           // DYNAMIC — active primary LLM
        string EmbeddingModel,
        bool RerankerLoaded,
        string RerankerModel,
        List<string> LoadErrors);
}
