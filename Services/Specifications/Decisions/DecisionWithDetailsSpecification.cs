using Domain.Entities;
using Shared.Dtos.Decisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Decisions
{
    public class DecisionWithDetailsSpecification : BaseSpecifications<int, Decision>
    {
        public DecisionWithDetailsSpecification(int caseId, DecisionFilterDto filter) 
        {
            ApplyFiltering(caseId, filter);
            ApplyIncludes();
            ApplyOrdering();
        }

        private void ApplyFiltering(int caseId, DecisionFilterDto filter)
        {
                Criteria = D =>
                            D.CaseId == caseId &&
                            (!filter.CourtSessionId.HasValue || D.CourtSessionId == filter.CourtSessionId.Value) &&
                            (!filter.IsFinal.HasValue || D.IsFinalVerdict == filter.IsFinal.Value);  
        }

        private void ApplyIncludes()
        {
                Includes.Add(D => D.Appeals);
                Includes.Add(D => D.CourtSession);
                Includes.Add(D => D.CourtSession.Case.Lawyers);
        }

        private void ApplyOrdering()
        {
            OrderByDescending = D => D.DecisionDate;
        }
    }
}
