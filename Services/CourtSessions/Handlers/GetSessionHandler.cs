using Domain.Contracts;
using Domain.Entities;
using Domain.Events.Sessions;
using Domain.Exceptions.NotFound;
using MediatR;
using Services.Specifications.CourtSessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CourtSessions.Handlers
{
    public class GetSessionHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetSessionEvent, CourtSession>
    {
        public async Task<CourtSession> Handle(GetSessionEvent request, CancellationToken cancellationToken)
        {
            var sessionSpec = new CourtSessionSpecifications(request.SessionId,
                                                                        false,
                                                                        false,
                                                                        true);

            var session = await _unitOfWork.GetRepository<int, CourtSession>().GetByIdAsync(sessionSpec);

            if (session is null)
                throw new SessionNotFoundException("Court session not found");
            return session;
        }
    }
}
