using Shared.Dtos.Decisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Decisions
{
    public interface IDecisionService
    {
        Task<DecisionResponse> CreateDecision(string lawyrId,int sessionId ,DecisionRequest request);
        Task<DecisionResponse> UpdateDecision(int caseId, int decisionId, string lawyerId, DecisionRequest request);
        Task<DecisionResponse> GetDecisionByIdAsync(int caseId, int decisionId, string lawyerId);
        Task<IEnumerable<DecisionResponse>> GetAllDecisionsAsync(DecisionFilterDto filter, string lawyerId);
        Task DeleteDecisionAsync(int caseId, int decisionId, string lawyerId);
    }
}
