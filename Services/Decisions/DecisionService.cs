using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Enums;
using Shared.Events.Decisions;
using Shared.Events.Sessions;
using Domain.Exceptions.BadRequest;
using Domain.Exceptions.NotFound;
using Domain.Exceptions.UnauthorizedException;
using MediatR;
using Services.Abstractions.Decisions;
using Services.Specifications.Cases;
using Services.Specifications.ChatBot;
using Services.Specifications.CourtSessions;
using Services.Specifications.Decisions;
using Shared.Dtos.CourtSessions;
using Shared.Dtos.Decisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Decisions
{
    public class DecisionService(IUnitOfWork _unitOfWork, IMapper _mapper, IMediator _mediator) : IDecisionService
    {
        public async Task<DecisionResponse> CreateDecision(string lawyerId, int sessionId, DecisionRequest request)
        {
            var session = await GetSession(sessionId);

            // Check if the lawyer is assigned to the case
            EnsureLawyerAuthorized(session.Case,lawyerId);


            var decision = _mapper.Map<Decision>(request);

            decision.CaseId = session.CaseId;
            decision.DecisionDate = session.SessionDate;
            decision.CourtSessionId = sessionId;
            decision.JudgeName = session.JudgeName;

            // Check if a decision already exists for this court session and if a final verdict already exists for this case
            await CheckIfDecisionAlreadyExistsAsync(session,sessionId);

            await _unitOfWork.GetRepository<int, Decision>().Add(decision);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new DecisionCreationFailedException("Failed to create Decision");

            await UpdateSessionState(session);

            if (decision.IsFinalVerdict)
                await UpdateCaseState(decision);

            if (request.NextSessionDate.HasValue)
                await PublishDecisionCreatedEvent(sessionId, request.NextSessionDate.Value);

            return _mapper.Map<DecisionResponse>(decision);
        }

        public async Task DeleteDecisionAsync(int caseId, int decisionId, string lawyerId)
        {
            var spec = new DecisionByIdSpecification(decisionId);
            var decision = await _unitOfWork.GetRepository<int, Decision>().GetByIdAsync(spec);
            if (decision is null)
                throw new DecisionNotFoundException("Decision is not found");

            EnsureLawyerAuthorized(decision.Case,lawyerId);

            if (decision.CaseId != caseId)
                throw new UnauthorizedException("You are not authorized to delete this decision");
         
            _unitOfWork.GetRepository<int, Decision>().Delete(decision);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new DecisionDeletionFailedException("Failed to delete decision");
        }

        public async Task<IEnumerable<DecisionResponse>> GetAllDecisionsAsync(int caseId, string lawyerId, DecisionFilterDto filter)
        {
            var caseSpec = new CaseSpecifications(caseId, false, false, false, false, false, true, false);
            var caseEntity = await _unitOfWork.GetRepository<int, Case>().GetByIdAsync(caseSpec);
            if (caseEntity is null)
                throw new CaseNotFoundException("Case not found");
            #region Old Code
            //foreach (var decision in decisions)
            //{
            //    if (decision.CaseId != caseId)
            //        throw new UnauthorizedException("You are not authorized to view these decisions");
            //}
            #endregion
            EnsureLawyerAuthorized(caseEntity, lawyerId);
            
            var spec = new DecisionWithDetailsSpecification(caseId, filter);
            var decisions = await _unitOfWork.GetRepository<int, Decision>().GetAllAsync(spec);

            if (decisions is null || !decisions.Any())
                throw new DecisionNotFoundException("No decisions found");

            return _mapper.Map<IEnumerable<DecisionResponse>>(decisions);
          
        }

        public async Task<DecisionResponse> GetDecisionByIdAsync(int caseId, int decisionId, string lawyerId)
        {
            var spec = new DecisionByIdSpecification(decisionId);
            var decision = await _unitOfWork.GetRepository<int, Decision>().GetByIdAsync(spec);
            if (decision is null)
                throw new DecisionNotFoundException("Decision is not found");
            EnsureLawyerAuthorized(decision.Case, lawyerId);

            if (decision.CaseId != caseId)
                throw new UnauthorizedException("You are not authorized to view this decision");

            return _mapper.Map<DecisionResponse>(decision);
                
        }

        public async Task<DecisionResponse> UpdateDecision(int caseId, int decisionId, string lawyerId, DecisionRequest request)
        {
            var spec = new DecisionByIdSpecification(decisionId);
            var decision = await _unitOfWork.GetRepository<int, Decision>().GetByIdAsync(spec);
            if (decision is null)
                throw new DecisionNotFoundException("Decision is not found");

            EnsureLawyerAuthorized(decision.Case, lawyerId);

            if (decision.CaseId != caseId)
                throw new UnauthorizedException("You are not authorized to update this decision");
            
            _mapper.Map(request, decision);
            _unitOfWork.GetRepository<int, Decision>().Update(decision);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new DecisionUpdateFailedBadRequestException("Failed to update decision");
            if (request.NextSessionDate.HasValue)
            {
                await _mediator.Publish(new DecisionCreatedEvent
                {
                    SessionId = decision.CourtSessionId,
                    NextSessionDate = request.NextSessionDate.Value
                });
            }

            return _mapper.Map<DecisionResponse>(decision);
            
        }



        private async Task PublishDecisionCreatedEvent(int sessionId, DateTime nextSessionDate)
        {
            await _mediator.Publish(new DecisionCreatedEvent
            {
                SessionId = sessionId,
                NextSessionDate = nextSessionDate
            });
        }
        private async Task CheckIfDecisionAlreadyExistsAsync(CourtSession session, int sessionId)
        {
            var spec = new DecisionWithCourtSessionsSpecification(session.CaseId);

            var decisions = await _unitOfWork.GetRepository<int, Decision>().GetAllAsync(spec);

            foreach (var existingDecision in decisions)
            {
                if (existingDecision.CourtSessionId == sessionId)
                    throw new DecisionCreationFailedException("A decision for this court session already exists");

                if (existingDecision.IsFinalVerdict)
                    throw new DecisionCreationFailedException("A final verdict already exists for this case");
            }
        }
        private async Task UpdateSessionState(CourtSession session) 
        {
            await _mediator.Publish(new SessionStateUpdatedEvent(session));
        }
        private async Task UpdateCaseState(Decision decision) 
        {
            await _mediator.Publish(new FinalVerdictReachedEvent { CaseId = decision.CaseId });
        }
        private async Task<CourtSession> GetSession(int sessionId) 
        {
             return await _mediator.Send(new GetSessionCommand(sessionId));
        }
        private static void EnsureLawyerAuthorized(Case caseEntity, string lawyerId)
        {
            if (!caseEntity.Lawyers.Any(l => l.Id == lawyerId))
                throw new UnauthorizedException("You are not authorized");
        }
    }
}
