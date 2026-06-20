using Domain.Entities;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.CaseParties
{
    public class CasePartiesWithFilterSpecification : BaseSpecifications<int,CaseParty>
    {
        public CasePartiesWithFilterSpecification(int caseId, int? PersonId, PartyRole? role)
        {
            Criteria = CP =>
                            (CP.CaseId == caseId) &&
                            (!PersonId.HasValue || CP.PersonId == PersonId.Value) &&
                            (!role.HasValue || CP.Role == role.Value);
            Includes.Add(cp => cp.Person);
            Includes.Add(cp => cp.Case);
        }
    }
}
