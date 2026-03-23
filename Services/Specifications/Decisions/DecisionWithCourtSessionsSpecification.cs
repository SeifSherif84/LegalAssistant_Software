using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Decisions
{
    public class DecisionWithCourtSessionsSpecification : BaseSpecifications<int, Decision>
    {
        public DecisionWithCourtSessionsSpecification(int caseId)
        {
            Criteria = D => D.CaseId == caseId;
            ApplyIncludes();
        }

        private void ApplyIncludes()
        {
            Includes.Add(D => D.CourtSession.Case.Lawyers);
        }
    }
}
