using Domain.Entities.ChatBotAIEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.ChatBot
{
    public class ChatSessionSpecifications : BaseSpecifications<int, ChatSession>
    {

        #region Get chatSession With Specific Id With Conditional Includes
        public ChatSessionSpecifications(int chatSessionId, bool includeMessages) : base()
        {
            ApplyFilteration(chatSessionId);
            ApplyIncludes(includeMessages);
        }

        private void ApplyFilteration(int chatSessionId)
        {
            Criteria = session => session.Id == chatSessionId;
        }

        private void ApplyIncludes(bool includeMessages)
        {
            if (includeMessages)
                Includes.Add(session => session.ChatMessages);
        }
        #endregion


        #region Get All ChatSessions Which Related To Specific Lawyer
        public ChatSessionSpecifications(string lawyerId, string? search) : base()
        {
            ApplyFilteration(lawyerId, search);
            ApplyOrderByDescending();
        }

        private void ApplyFilteration(string lawyerId, string? search)
        {
            Criteria = session => session.LawyerId == lawyerId &&
                                  (string.IsNullOrEmpty(search) || session.Title.ToLower().Contains(search.ToLower()));
        }

        private void ApplyOrderByDescending()
        {
            OrderByDescending = session => session.CreatedAt;
        } 
        #endregion


    }
}
