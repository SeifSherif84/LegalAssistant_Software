using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Exceptions.BadRequest;
using Domain.Exceptions.NotFound;
using Services.Abstractions.CourtSessions;
using Services.Specifications.Cases;
using Shared.Dtos.Cases;
using Shared.Dtos.CourtSessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CourtSessions
{
    public class CourtSessionService(IMapper _mapper,
                                     IUnitOfWork _unitOfWork) : ICourtSessionService
    {
        public async Task AddSessionAsync(int caseId, string lawyerId, CreateCourtSession createCourtSession)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            // 1. Check CaseId Existence
            var caseSpecifications = new CaseSpecifications(caseId, false,
                                                                    false,
                                                                    false,
                                                                    false,
                                                                    false,
                                                                    true,
                                                                    false);
            var caseEntity = await _unitOfWork.GetRepository<int, Case>().GetByIdAsync(caseSpecifications);
            if (caseEntity is null)
                throw new CaseNotFoundException($"Case with id : {caseId} not found.");


            // 2. Check if the case belongs to this lawyer
            if (caseEntity.Lawyers.Any(L => L.Id == lawyerId))
            {
                var newCourtSession = _mapper.Map<CourtSession>(createCourtSession);
                newCourtSession.SessionStatus = SessionStatus.Scheduled;
                newCourtSession.CaseId = caseId;
                await _unitOfWork.GetRepository<int, CourtSession>().Add(newCourtSession);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result <= 0)
                    throw new CaseUpdateFailedException("Failed to Add Session. Please try again later.");
            }
            else
                throw new UnauthorizedAccessException("You don't have permission to Add Session To this case.");
        }



        public async Task<IEnumerable<CourtSessionResponse>> GetAllSessionsAsync(int caseId, string lawyerId)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            // 1. Check CaseId Existence
            var caseSpecifications = new CaseSpecifications(caseId, false,
                                                                    true,
                                                                    false,
                                                                    false,
                                                                    false,
                                                                    true,
                                                                    false);
            var caseEntity = await _unitOfWork.GetRepository<int, Case>().GetByIdAsync(caseSpecifications);
            if (caseEntity is null)
                throw new CaseNotFoundException($"Case with id : {caseId} not found.");


            // 2. Check if the case belongs to this lawyer
            if (caseEntity.Lawyers.Any(L => L.Id == lawyerId))
            {
                var sessions = _mapper.Map<IEnumerable<CourtSessionResponse>>(caseEntity.CourtSessions);
                return sessions;
            }

            else
                throw new UnauthorizedAccessException("You don't have permission to Load Sessions for this case.");
        }


    }
}
