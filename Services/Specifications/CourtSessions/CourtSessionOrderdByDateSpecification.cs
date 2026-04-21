using Domain.Entities;
using MailKit.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.CourtSessions
{
    public class CourtSessionOrderdByDateSpecification : BaseSpecifications<int, CourtSession>
    {
        public CourtSessionOrderdByDateSpecification(int caseId, DateTime todayDate)
        {
            Criteria = S => S.CaseId == caseId && S.SessionDate >= todayDate;
            ApplyInclude();
            ApplyOrderBy();
            // Filter sessions by caseId and date
        }
        private void ApplyInclude()
        {
            Includes.Add(S => S.Case.Lawyers);
        }
        
        private void ApplyOrderBy()
        {
           OrderBy = S => S.SessionDate;
        }
    }
}
