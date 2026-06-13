using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Enums;
using Shared.Events.Sessions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CourtSessions.Handlers
{
    public class UpdateSessionStateHandler(IUnitOfWork _unitOfWork) : INotificationHandler<SessionStateUpdatedEvent>
    {
        public async Task Handle(SessionStateUpdatedEvent notification, CancellationToken cancellationToken)
        {
            notification.Session.SessionStatus = SessionStatus.مكتملة;
            _unitOfWork.GetRepository<int, CourtSession>().Update(notification.Session);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
