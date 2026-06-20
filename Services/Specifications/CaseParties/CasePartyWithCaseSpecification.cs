using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.CaseParties
{
    public class CasePartyWithCaseSpecification : BaseSpecifications<int, CaseParty>
    {
        public CasePartyWithCaseSpecification(int caseId, int casePartyId) 
        {
            Criteria = CP =>
                           (CP.CaseId == caseId) &&
                           (CP.Id == casePartyId);
            ApplyIncludes();
        }
        private void ApplyIncludes()
        {
            Includes.Add(CP => CP.Case.Lawyers);
        }

    }
}
