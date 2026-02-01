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
            ApplyFilteration(lawyerId);
        }

        private void ApplyFilteration(string lawyerId)
        {
            Criteria = C => C.Lawyers.Any(L => L.Id == lawyerId);
        }

        
    }
}
