using Domain.Entities.ChatBotAIEntities;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.ChatBotAIEntities.Enums;

namespace Shared.Dtos.ChatBot
{
    public class ChatSessionResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
