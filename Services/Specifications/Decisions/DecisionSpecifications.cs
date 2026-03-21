using Domain.Entities;
using Shared.Dtos.Decisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Decisions
{
    public class DecisionSpecifications : BaseSpecifications<int, Decision>
    {
        public DecisionSpecifications(int id,bool includeAppeals, bool includeCourtSession, bool includeCaseWithLawyers) : base()
        {
            ApplyfilteringById(id);
            ApplyIncludes(includeAppeals,includeCourtSession,includeCaseWithLawyers);
        }
        public DecisionSpecifications(DecisionFilterDto? filter, bool includeAppeals, bool includeCourtSession, bool includeCaseWithLawyers, bool applyFiltering, bool applyOrdreing) : base()
        {
            ApplyFiltering(filter, applyFiltering);
            ApplyIncludes(includeAppeals, includeCourtSession, includeCaseWithLawyers);
            ApplyOrdering(applyOrdreing);
        }
        private void ApplyIncludes(bool includeAppeals, bool IncludeCourtSession, bool includeCaseWithLawyers)
        {
                if(includeAppeals)
                Includes.Add(D => D.Appeals);
                if (IncludeCourtSession)
                Includes.Add(D => D.CourtSession);
                if (includeCaseWithLawyers)
                Includes.Add(D => D.CourtSession.Case.Lawyers);
        }
        private void ApplyFiltering(DecisionFilterDto filter, bool applyFiltering)
        {
            if (applyFiltering)
            {
                Criteria = D =>
                            (filter.CaseId == 0 || D.CaseId == filter.CaseId) &&
                            (!filter.CourtSessionId.HasValue || D.CourtSessionId == filter.CourtSessionId.Value) &&
                            (!filter.IsFinal.HasValue || D.IsFinalVerdict == filter.IsFinal.Value);
            }
        }
        private void ApplyOrdering(bool applyOrdering)
        {
            if (applyOrdering)
            {
                OrderByDescending = D => D.DecisionDate;
            }
        }
        private void ApplyfilteringById(int id)
        {
            Criteria = D => D.Id == id;
        }
    }
}
