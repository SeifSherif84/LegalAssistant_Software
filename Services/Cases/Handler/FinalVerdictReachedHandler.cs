using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Events.Decisions;
using MediatR;
using Services.Specifications.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Cases.Handler
{
    public class FinalVerdictReachedHandler(IUnitOfWork _unitOfWork) : INotificationHandler<FinalVerdictReachedEvent>
    {
        public async Task Handle(FinalVerdictReachedEvent notification, CancellationToken cancellationToken)
        {
            var caseSpec = new CaseSpecifications(notification.CaseId, false, false, false, false, false, true, false);
            var caseEntity = await _unitOfWork.GetRepository<int, Case>().GetByIdAsync(caseSpec);
            if (caseEntity is not null)
            {
                caseEntity.Status = CaseStatus.Closed;
                _unitOfWork.GetRepository<int, Case>().Update(caseEntity);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
