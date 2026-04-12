using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Sessions
{
    public class GetSessionEvent : IRequest<CourtSession>
    {
        public int SessionId { get; set; }
        public GetSessionEvent(int sessionId)
        {
            SessionId = sessionId;
        }
    }
}
