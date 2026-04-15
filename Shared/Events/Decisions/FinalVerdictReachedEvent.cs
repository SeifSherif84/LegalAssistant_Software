using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.Decisions
{
    public class FinalVerdictReachedEvent : INotification
    {
        public int CaseId { get; set; }
    }
}
