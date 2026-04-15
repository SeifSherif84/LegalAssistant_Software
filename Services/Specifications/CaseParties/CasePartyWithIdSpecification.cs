using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.CaseParties
{
    public class CasePartyWithIdSpecification : BaseSpecifications<int, CaseParty>
    {
        public CasePartyWithIdSpecification(int caseId, int casePartyId)
        {
            Criteria = CP =>
                           (CP.CaseId == caseId) &&
                           (CP.Id == casePartyId);
        }
    }
}
