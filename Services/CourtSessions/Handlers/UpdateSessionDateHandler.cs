using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Enums;
using Shared.Events.Decisions;
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
    public class UpdateSessionDateHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : INotificationHandler<DecisionCreatedEvent>
    {
        public async Task Handle(DecisionCreatedEvent notification, CancellationToken cancellationToken)
        {
            var spec = new CourtSessionSpecifications(notification.SessionId, false, false, false);
            var currentSession = await _unitOfWork.GetRepository<int, CourtSession>().GetByIdAsync(spec);

            if (currentSession is null) 
                throw new SessionNotFoundException("Current session not found");

            int finalNextSessionId;

            
            if (currentSession.NextSessionId.HasValue)
            {
                var nextSpec = new CourtSessionSpecifications(currentSession.NextSessionId.Value, false, false, false);
                var nextSession = await _unitOfWork.GetRepository<int, CourtSession>().GetByIdAsync(nextSpec);

                if (nextSession is null) 
                    throw new SessionNotFoundException("Linked next session not found");
                nextSession.SessionDate = notification.NextSessionDate;
                nextSession.SessionStatus = SessionStatus.Scheduled;

                _unitOfWork.GetRepository<int, CourtSession>().Update(nextSession);
                finalNextSessionId = nextSession.Id;
            }
            else
            {
                var newSession = _mapper.Map<CourtSession>(currentSession);
                newSession.SessionDate = notification.NextSessionDate;
                newSession.SessionStatus = SessionStatus.Scheduled;

                await _unitOfWork.GetRepository<int, CourtSession>().Add(newSession);
                await _unitOfWork.SaveChangesAsync();
                finalNextSessionId = newSession.Id;
            }

            currentSession.NextSessionId = finalNextSessionId;
            currentSession.SessionStatus = SessionStatus.Postponed;

            _unitOfWork.GetRepository<int, CourtSession>().Update(currentSession);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
