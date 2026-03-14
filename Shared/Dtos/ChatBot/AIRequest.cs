using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot
{
    #region Old
    //public class AIRequest
    //{
    //    public List<AIMessage> Messages { get; set; }
    //} 
    #endregion


    public class AIRequest

    {
        [JsonPropertyName("question")]
        public string Question { get; set; } = string.Empty;


        [JsonPropertyName("top_k")]
        public int MaxRelevantChunksCount { get; set; } = 5;


        [JsonPropertyName("messages")]
        public List<AIMessage> ChatHistory { get; set; } = new();
    }

}
