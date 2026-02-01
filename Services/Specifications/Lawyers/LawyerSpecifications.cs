using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Lawyers
{
    public class LawyerSpecifications : BaseSpecifications<string, Lawyer>
    {
        // Get lawyer by Id Without includes
        public LawyerSpecifications(string lawyerId)
        {
            ApplyFilteration(lawyerId);
        }

        private void ApplyFilteration(string lawyerId)
        {
            Criteria = L => L.Id == lawyerId;
        }

    }
}
