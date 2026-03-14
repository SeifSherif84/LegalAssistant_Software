using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class DecisionSpecificationsDashboard : BaseSpecifications<int, Decision>
    {
        public DecisionSpecificationsDashboard(string lawyerId,
                                               DateTime today,
                                               DateTime nextWeek,
                                               bool includesSession,
                                               bool includesCase)
        {
            ApplyFilteration(lawyerId, today, nextWeek);
            ApplyIncludes(includesSession, includesCase);
        }

        private void ApplyFilteration(string lawyerId, DateTime today, DateTime nextWeek)
        {
            Criteria = D => D.CourtSession.Case.Lawyers.Any(L => L.Id == lawyerId) &&
                                                            D.AppealDeadline >= today &&
                                                            D.AppealDeadline <= nextWeek;
        }

        private void ApplyIncludes(bool includesSession, bool includesCase)
        {
            if (includesSession) 
                Includes.Add(D => D.CourtSession);
            if (includesCase)
                Includes.Add(D => D.CourtSession.Case);
        }
    }
}
