using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Cases
{
    public class CaseSpecifications : BaseSpecifications<int, Case>
    {
        // Get all cases for a specific lawyer Without includes
        public CaseSpecifications(string lawyerId) : base()
        {
            ApplyFilterationToGetAllCaseForSpecificLawyer(lawyerId);
        }

        // Get case by Id Without includes
        public CaseSpecifications(int caseId) : base()
        {
            ApplyFilterationToGetCaseById(caseId);
        }

        // Get all cases for a specific lawyer Without includes
        private void ApplyFilterationToGetAllCaseForSpecificLawyer(string lawyerId)
        {
            Criteria = C => C.Lawyers.Any(L => L.Id == lawyerId);
        }

        // Get case by Id Without includes
        private void ApplyFilterationToGetCaseById(int caseId)
        {
            Criteria = C => C.Id == caseId;
        }


    }
}
