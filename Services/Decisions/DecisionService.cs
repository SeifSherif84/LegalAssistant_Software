using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Events.Decisions;
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
        public async Task<DecisionResponse> CreateDecision(string lawyerId,int sessionId , DecisionRequest request)
        {
            var sessionSpec = new CourtSessionSpecifications(sessionId,
                                                                        false,
                                                                        false,
                                                                        true);
            var session = await _unitOfWork.GetRepository<int, CourtSession>().GetByIdAsync(sessionSpec);
            if (session is null)
                throw new SessionNotFoundException("Court session not found");
            var decision = _mapper.Map<Decision>(request);
            decision.CaseId = session.CaseId;
            decision.DecisionDate = session.SessionDate;
            decision.CourtSessionId = sessionId;
            // Check if a decision already exists for this court session and if a final verdict already exists for this case
            var filter = new DecisionFilterDto
            {
                CaseId = session.CaseId, 
            };
     
            var spec = new DecisionSpecifications(filter, false, true, false, true, false);
            var decisions = await _unitOfWork.GetRepository<int, Decision>().GetAllAsync(spec);
            foreach (var existingDecision in decisions)
            {
                if (existingDecision.CourtSessionId == sessionId)
                    throw new DecisionCreationFailedException("A decision for this court session already exists");
                if(existingDecision.IsFinalVerdict)
                    throw new DecisionCreationFailedException("A final verdict already exists for this case");
            }

            // Check if the lawyer is assigned to the case
            if (session.Case.Lawyers.Any(L => L.Id == lawyerId))
            {
                
                await _unitOfWork.GetRepository<int, Decision>().Add(decision);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result <= 0)
                    throw new DecisionCreationFailedException("Failed to create Decision");

                if (request.NextSessionDate.HasValue)
                {
                    await _mediator.Publish(new DecisionCreatedEvent
                    {
                        SessionId = sessionId,
                        NextSessionDate = request.NextSessionDate.Value
                    });
                }

                var decisionSpec = new DecisionSpecifications(decision.Id,true,true,false);
                var createdDecision = await _unitOfWork.GetRepository<int, Decision>().GetByIdAsync(decisionSpec);
                if (createdDecision is null)
                    throw new DecisionNotFoundException("Decision is not found");

                return _mapper.Map<DecisionResponse>(createdDecision);
            }
            else
            {
                throw new UnauthorizedException("You are not authorized to create a decision for this case");
            }

        }

        public async Task DeleteDecisionAsync(int caseId, int decisionId, string lawyerId)
        {
            var spec = new DecisionSpecifications(decisionId, true, true, true);
            var decision = await _unitOfWork.GetRepository<int, Decision>().GetByIdAsync(spec);
            if (decision is null)
                throw new DecisionNotFoundException("Decision is not found");
            if (decision.Case.Lawyers.Any(L => L.Id == lawyerId) && decision.CaseId == caseId)
            {
                _unitOfWork.GetRepository<int, Decision>().Delete(decision);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result <= 0)
                    throw new DecisionDeletionFailedException("Failed to delete decision");
            }
            else
                throw new UnauthorizedException("You are not authorized to delete this decision");

        }

        public async Task<IEnumerable<DecisionResponse>> GetAllDecisionsAsync(DecisionFilterDto filter,string lawyerId)
        {
            var spec = new DecisionSpecifications(filter, true, true,true,true,true);
            var decisions = await _unitOfWork.GetRepository<int, Decision>().GetAllAsync(spec);

            if (decisions is null || !decisions.Any())
                throw new DecisionNotFoundException("No decisions found");
            var caseSpec = new CaseSpecifications(filter.CaseId,false,false,false,false,false,true,false);
            var caseEntity = await _unitOfWork.GetRepository<int, Case>().GetByIdAsync(caseSpec);
            if (caseEntity is null)
                throw new CaseNotFoundException("Case not found");
            foreach (var decision in decisions)
            {
                if (decision.CaseId != filter.CaseId)
                    throw new UnauthorizedException("You are not authorized to view these decisions");
            }
            if (caseEntity.Lawyers.Any(L => L.Id == lawyerId))
                return _mapper.Map<IEnumerable<DecisionResponse>>(decisions);
            else
                throw new UnauthorizedException("You are not authorized to view these decisions");
        }

        public async Task<DecisionResponse> GetDecisionByIdAsync(int caseId, int decisionId, string lawyerId)
        {
            var spec = new DecisionSpecifications(decisionId,true,true,true);
            var decision = await _unitOfWork.GetRepository<int, Decision>().GetByIdAsync(spec);
            if (decision is null)
                throw new DecisionNotFoundException("Decision is not found");

            if (decision.Case.Lawyers.Any(L=>L.Id==lawyerId) && decision.CaseId==caseId)
                return _mapper.Map<DecisionResponse>(decision);
            else
                throw new UnauthorizedException("You are not authorized to view this decision");
        }

        public async Task<DecisionResponse> UpdateDecision(int caseId, int decisionId, string lawyerId, DecisionRequest request)
        {
            var spec = new DecisionSpecifications(decisionId, true, true, true);
            var decision = await _unitOfWork.GetRepository<int, Decision>().GetByIdAsync(spec);
            if (decision is null)
                throw new DecisionNotFoundException("Decision is not found");
            if (decision.CaseId == caseId)
            {
                _mapper.Map(request, decision);
                _unitOfWork.GetRepository<int, Decision>().Update(decision);
                var result = await _unitOfWork.SaveChangesAsync();
                if (result <= 0)
                    throw new DecisionUpdateFailedBadRequestException("Failed to update decision");
                if (decision.Case.Lawyers.Any(L => L.Id == lawyerId))
                    return _mapper.Map<DecisionResponse>(decision);
                else
                    throw new UnauthorizedException("You are not authorized to update this decision");
            }
            else
                throw new UnauthorizedException("You are not authorized");
        }
    }
}
