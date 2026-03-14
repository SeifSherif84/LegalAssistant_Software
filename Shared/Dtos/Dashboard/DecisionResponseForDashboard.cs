using Domain.Entities.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Dashboard
{
    public class DecisionResponseForDashboard
    {
        public DateTime DecisionDate { get; set; } 
        public DecisionType DecisionType { get; set; } 
        public string SentenceText { get; set; } 
        public DateTime AppealDeadline { get; set; } 
        public string? JudgeName { get; set; } 
        public SentenceType SentenceType { get; set; } 


        public DateTime sessionDate { get; set; }
        public string sessionCourtName { get; set; }
        public string sessionCourtRoom { get; set; }
        public string sessionFloor { get; set; }
        public SessionType sessionType { get; set; }
        public SessionStatus sessionStatus { get; set; }


        public string caseTitle { get; set; }
        public string caseFileNumber { get; set; }
        public CaseStatus caseStatus { get; set; }

    }
}
