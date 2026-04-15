using Domain.Entities.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.Decisions
{
    public class DecisionCreatedEvent : INotification
    {
        public int SessionId { get; set; }
        public DateTime NextSessionDate { get; set; }
    }
}
