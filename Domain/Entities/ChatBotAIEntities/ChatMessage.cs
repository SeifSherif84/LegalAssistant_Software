using Domain.Contracts;
using Domain.Entities.ChatBotAIEntities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ChatBotAIEntities
{
    public class ChatMessage : ISoftDelete
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public int ChatSessionId { get; set; }
        public ChatSession ChatSession { get; set; }

        public MessageSender MessageSender { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
