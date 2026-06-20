using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot
{
    #region Old
    //public class AIResponse
    //{
    //    public string BookName { get; set; }
    //    public string? Answer { get; set; }
    //} 
    #endregion

    public class AIResponse

    {
        [JsonPropertyName("question")]
        public string EchoedQuestion { get; set; } = string.Empty;


        [JsonPropertyName("answer")]
        public string GeneratedAnswer { get; set; } = string.Empty;


        [JsonPropertyName("sources")]
        public List<AISource> References { get; set; } = new();


        [JsonPropertyName("processing_ms")]
        public int InferenceTimeMs { get; set; }

        public AISource? TopSource =>
            References.OrderByDescending(reference => reference.MatchConfidence).FirstOrDefault();

    }


}
