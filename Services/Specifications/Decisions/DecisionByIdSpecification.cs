using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Decisions
{
    public class DecisionByIdSpecification : BaseSpecifications<int, Decision>
    {
        public DecisionByIdSpecification(int id) 
        {
            Criteria = D => D.Id == id;
            ApplyIncludes();
        }

        private void ApplyIncludes()
        {
                Includes.Add(D => D.Appeals);
                Includes.Add(D => D.CourtSession);
                Includes.Add(D => D.CourtSession.Case.Lawyers);
        }
    }
}
