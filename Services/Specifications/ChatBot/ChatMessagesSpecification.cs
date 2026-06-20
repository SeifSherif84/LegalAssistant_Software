using Domain.Entities.ChatBotAIEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.ChatBot
{
    public class ChatMessagesSpecification : BaseSpecifications<int, ChatMessage>
    {
        #region ForHistory
        public ChatMessagesSpecification(int chatSessionId) : base()
        {
            ApplyFileration(chatSessionId);
            ApplyOrderByDescending();
            ApplyPagination(10);
        }

        private void ApplyFileration(int chatSessionId)
        {
            Criteria = Message => Message.ChatSessionId == chatSessionId;
        }

        private void ApplyOrderByDescending()
        {
            OrderByDescending = Message => Message.CreatedAt;
        }

        private void ApplyPagination(int numberOfMessageTaken)
        {
            IsPaginationEnabled = true;
            Take = numberOfMessageTaken;
        }
        #endregion


    }
}
