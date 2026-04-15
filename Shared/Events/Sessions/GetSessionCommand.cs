using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.Sessions
{
    public class GetSessionCommand : IRequest<CourtSession>
    {
        public int SessionId { get; set; }
        public GetSessionCommand(int sessionId)
        {
            SessionId = sessionId;
        }
    }
}
