using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using MediatR;
using Services.Specifications;
using Services.Specifications.Cases;
using Shared.Dtos.Cases;
using Shared.Events.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Cases.Handler
{
    public class GetCaseHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetCaseCommand, Case>
    {
        public async Task<Case> Handle(GetCaseCommand request, CancellationToken cancellationToken)
        {
            var caseSpecifications = new CaseSpecifications(request.CaseId, false,
                                                                    false,
                                                                    false,
                                                                    false,
                                                                    false,
                                                                    true,
                                                                    false);
            var caseEntity = await _unitOfWork.GetRepository<int, Case>().GetByIdAsync(caseSpecifications);
            if (caseEntity == null)
                throw new Exception("Case not found");
            
            return caseEntity;
        }
    }
}
