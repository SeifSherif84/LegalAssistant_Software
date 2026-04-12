using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Sessions
{
    public class SessionStateUpdatedEvent : INotification
    {
        public CourtSession Session { get; set; } 
        public SessionStateUpdatedEvent(CourtSession session)
        {
            Session = session;
        }
    }
}
