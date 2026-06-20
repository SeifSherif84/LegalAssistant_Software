using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.CaseParties
{
    public class CasePartiesWithLawyerIdSpecification : BaseSpecifications<int ,CaseParty>
    {
        public CasePartiesWithLawyerIdSpecification(string lawyerId)
        {
            Criteria = CP => CP.LawyerId == lawyerId;
             Includes.Add(cp => cp.Person);
             Includes.Add(cp => cp.Case.Lawyers);
        }
    }
}
