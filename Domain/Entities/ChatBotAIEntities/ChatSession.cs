using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ChatBotAIEntities
{
    public class ChatSession : ISoftDelete
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }

        public string LawyerId { get; set; }
        public Lawyer Lawyer { get; set; }


        public List<ChatMessage> ChatMessages { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
