using Domain.Entities.ChatBotAIEntities.Enums;
using Domain.Entities.ChatBotAIEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot
{
    public class ChatMessagesResponse
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public MessageSender MessageSender { get; set; }
    }
}
