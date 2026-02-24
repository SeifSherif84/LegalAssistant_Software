using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.CourtSessions
{
    public class CourtSessionResponseDashboard
    {
        public DateTime SessionDate { get; set; }
        public string CourtName { get; set; }
        public string CourtRoom { get; set; }
        public string Floor { get; set; }
        public string JudgeName { get; set; }

        public SessionType SessionType { get; set; }
        public SessionStatus SessionStatus { get; set; }

        public string? AdjournmentReason { get; set; }
        public string? CancelledReason { get; set; }

        public string? Notes { get; set; }


        public string caseTitle { get; set; }
        public string caseFileNumber { get; set; }
        public CaseStatus caseStatus { get; set; }
    }
}
