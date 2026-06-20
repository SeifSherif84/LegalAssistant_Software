using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot
{
    public class AISource

    {
        [JsonPropertyName("name")]
        public string CleanName { get; set; } = string.Empty;


        [JsonPropertyName("file_name")]
        public string FilePath { get; set; } = string.Empty;


        [JsonPropertyName("category")]
        public string LegalBranch { get; set; } = string.Empty;


        [JsonPropertyName("collection")]
        public string LegalEncyclopedia { get; set; } = string.Empty;


        [JsonPropertyName("score")]
        public double MatchConfidence { get; set; }

    }
}
