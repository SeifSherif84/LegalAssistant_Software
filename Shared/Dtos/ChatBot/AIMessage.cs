using Shared.Dtos.ChatBot;
using Shared.Dtos.ChatBot.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot
{
    #region Old
    //public class AIMessage
    //{
    //    public MessageRole Role { get; set; } // "user" or "assistant"
    //    public string Content { get; set; }
    //} 
    #endregion

    public class AIMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty;

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }

}


