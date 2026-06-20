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
        // Get lawyer by Id with conditional includes
        public LawyerSpecifications(string lawyerId,                                    
                                    bool includeCases)
        {
            ApplyFilteration(lawyerId);
            ApplyIncludes(includeCases);

            #region old Dashboard
            //ApplyIncludes(includeCasesWithSessionsWithDecisionsWithAppeals);  
            #endregion
        }

        private void ApplyFilteration(string lawyerId)
        {
            Criteria = L => L.Id == lawyerId;
        }

        private void ApplyIncludes(bool includeCases)
        {
            if (includeCases)
                Includes.Add(L => L.Cases);
        }

        #region old Dashboard
        //private void ApplyIncludes(bool includeCasesWithSessionsWithDecisions)
        //{
        //    if (includeCasesWithSessionsWithDecisions)
        //        Includes.Add(L => L.Cases.SelectMany(C => C.CourtSessions)
        //                                 .SelectMany(CS => CS.Decisions)
        //                                 .SelectMany(D => D.Appeals)
        //                                 );
        //} 
        #endregion

    }
}
