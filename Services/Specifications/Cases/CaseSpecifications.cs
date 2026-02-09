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


        // Get all cases for a specific lawyer Without includes
        private void ApplyFilterationToGetAllCaseForSpecificLawyer(string lawyerId)
        {
            Criteria = C => C.Lawyers.Any(L => L.Id == lawyerId);
        }



        // Get case by Id With conditional includes
        public CaseSpecifications(int caseId, bool includeDocuments, 
                                              bool includeSessions,
                                              bool includeDecisions,
                                              bool includeAppeal,
                                              bool includeCourtParties,
                                              bool includeLawyers,
                                              bool includeAiAnalysises) : base()
        {
            ApplyFilterationToGetCaseById(caseId);
            ApplyIncludesForSpecificCase(includeDocuments,
                                         includeSessions,
                                         includeDecisions,
                                         includeAppeal,
                                         includeCourtParties,
                                         includeLawyers,
                                         includeAiAnalysises);
        }

        // Get case by Id With conditional includes
        private void ApplyFilterationToGetCaseById(int caseId)
        {
            Criteria = C => C.Id == caseId;
        }


        private void ApplyIncludesForSpecificCase(bool includeDocuments,
                                                  bool includeSessions,
                                                  bool includeDecisions,
                                                  bool includeAppeal,
                                                  bool includeCourtParties,
                                                  bool includeLawyers,
                                                  bool includeAiAnalysises)
        {
            if (includeDocuments)
                Includes.Add(C => C.Documents);
            if (includeSessions)
                Includes.Add(C => C.CourtSessions);
            if (includeDecisions)
                Includes.Add(C => C.Decisions);
            if (includeAppeal)
                Includes.Add(C => C.Appeals);
            if (includeCourtParties)
                Includes.Add(C => C.CaseParties);
            if (includeLawyers)
                Includes.Add(C => C.Lawyers);
            if (includeAiAnalysises)
                Includes.Add(C => C.AiAnalyses);
        }


    }
}
