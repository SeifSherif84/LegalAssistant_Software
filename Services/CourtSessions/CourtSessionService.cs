  using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Exceptions.BadRequest;
using Domain.Exceptions.NotFound;
using Services.Abstractions.CourtSessions;
using Services.Specifications.Cases;
using Services.Specifications.CourtSessions;
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
                    throw new CaseUpdatedFailedException("Failed to Add Session. Please try again later.");
            }
            else
                throw new UnauthorizedAccessException("You don't have permission to Add Session To this case.");
        }


        public async Task DeleteSessionAsync(int sessionId, string lawyerId)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            // 2. Check Session Existence
            var sessionSpecifications = new CourtSessionSpecifications(sessionId, false,
                                                                                  false,
                                                                                  true);
            var sessionEntity = await _unitOfWork.GetRepository<int, CourtSession>().GetByIdAsync(sessionSpecifications);
            if (sessionEntity is null)
                throw new SessionNotFoundException($"Session with id : {sessionId} not found.");


            // 3. Check if the session belongs to a case of this lawyer
            if (sessionEntity.Case.Lawyers.Any(L => L.Id == lawyerId))
            {
                sessionEntity.IsDeleted = true;
                sessionEntity.DeletedAt = DateTime.UtcNow;
                var result = await _unitOfWork.SaveChangesAsync();
                if (result <= 0)
                    throw new CaseUpdatedFailedException("Failed to Delete Session. Please try again later.");
            }
            else
                throw new UnauthorizedAccessException("You don't have permission to Delete Session of this case.");

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


        public async Task UpdateSessionAsync(int sessionId, string lawyerId, UpdateCourtSession updateCourtSession)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            // 2. Check Session Existence
            var sessionSpecifications = new CourtSessionSpecifications(sessionId, false,
                                                                                  false,
                                                                                  true);
            var sessionEntity = await _unitOfWork.GetRepository<int, CourtSession>().GetByIdAsync(sessionSpecifications);
            if (sessionEntity is null)
                throw new SessionNotFoundException($"Session with id : {sessionId} not found.");


            // 3. Check if the session belongs to a case of this lawyer
            if (sessionEntity.Case.Lawyers.Any(L => L.Id == lawyerId))
            {
                _mapper.Map(updateCourtSession, sessionEntity);
                _unitOfWork.GetRepository<int, CourtSession>().Update(sessionEntity);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result <= 0)
                    throw new SessionUpdatedFailedException("Failed to Update Session. Please try again later.");
            }
            else
                throw new UnauthorizedAccessException("You don't have permission to Update Session of this case.");
        }


        public async Task<CourtSessionResponse> GetSessionByIdAsync(int sessionId, string lawyerId)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            // 2. Check Session Existence
            var sessionSpecifications = new CourtSessionSpecifications(sessionId, false,
                                                                                  false,
                                                                                  true);
            var sessionEntity = await _unitOfWork.GetRepository<int, CourtSession>().GetByIdAsync(sessionSpecifications);
            if (sessionEntity is null)
                throw new SessionNotFoundException($"Session with id : {sessionId} not found.");


            // 3. Check if the session belongs to a case of this lawyer
            if (sessionEntity.Case.Lawyers.Any(L => L.Id == lawyerId))
                return _mapper.Map<CourtSessionResponse>(sessionEntity);
            else
                throw new UnauthorizedAccessException("You don't have permission to display this session.");
        }




        public async Task<IEnumerable<CourtSessionResponseDashboard>> GetLawyerSessionsAsync(string lawyerId, string period)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            DateTime? startDate = null;
            DateTime? endDate = null;

            if (period.ToLower() == "today")
            {
                startDate = DateTime.Now.Date;
                endDate = DateTime.Now.Date.AddDays(1).AddTicks(-1);
            }
            else if (period.ToLower() == "week")
            {
                startDate = DateTime.Now.Date;
                endDate = DateTime.Now.Date.AddDays(7);
            }

            var spec = new CourtSessionSpecifications(lawyerId, startDate, endDate, true);
            var sessions = await _unitOfWork.GetRepository<int, CourtSession>().GetAllAsync(spec);
            if (sessions is null || !sessions.Any())
                throw new SessionNotFoundException("No sessions found for the specified period.");

            return _mapper.Map<IEnumerable<CourtSessionResponseDashboard>>(sessions);
        }

    }
}
