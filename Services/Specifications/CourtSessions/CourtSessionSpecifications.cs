using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.CourtSessions
{
    public class CourtSessionSpecifications : BaseSpecifications<int, CourtSession>
    {
        // Get session by Id with conditional includes
        public CourtSessionSpecifications(int sessionId, bool includeDecisions,
                                                         bool includeDocuments,
                                                         bool includeCaseWithLawyers) : base()
        {
            ApplyFilterationToGetSessionById(sessionId);
            ApplyInclude(includeDecisions, includeDocuments, includeCaseWithLawyers);
        }

        private void ApplyFilterationToGetSessionById(int sessionId)
        {
            Criteria = S => S.Id == sessionId;
        }

        private void ApplyInclude(bool includeDecisions,
                                  bool includeDocuments,
                                  bool includeCaseWithLawyers)
        {
            if (includeDecisions)
                Includes.Add(S => S.Decisions);
            if (includeDocuments)
                Includes.Add(S => S.Documents);
            if (includeCaseWithLawyers)
                Includes.Add(S => S.Case.Lawyers);
        }




        // Get all sessions of all cases which related ro specific lawyer with optional date range filter
        public CourtSessionSpecifications(string lawyerId,
                                          DateTime? startDate,
                                          DateTime? endDate,
                                          bool includeCase) : base()
        {
            ApplyFilterationToGetSessionsOfSpecificLawyer(lawyerId, startDate, endDate);
            ApplyInclude(includeCase);
        }

        private void ApplyFilterationToGetSessionsOfSpecificLawyer(string lawyerId, DateTime? startDate, DateTime? endDate)
        {
            Criteria = S =>
                            (S.Case.Lawyers.Any(L => L.Id == lawyerId)) &&
                            (!startDate.HasValue || S.SessionDate >= startDate.Value) &&
                            (!endDate.HasValue || S.SessionDate <= endDate.Value);
        }

        private void ApplyInclude(bool includeCase)
        {
            if(includeCase)
                Includes.Add(S => S.Case);
        }

    }
}
