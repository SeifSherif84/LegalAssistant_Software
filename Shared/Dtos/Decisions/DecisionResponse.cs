using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Decisions
{
    public class DecisionResponse
    {
        // Basic Info
        public int Id { get; set; }
        public DateTime DecisionDate { get; set; }
        public DateTime? NextSessionDate { get; set; }
        public string DecisionType { get; set; }      // مش Enum — بترجع string للـ client
        public string SentenceType { get; set; }
        public string SentenceText { get; set; }
        public string? JudgeName { get; set; }

        // Appeal Info
        public bool Appealable { get; set; }
        public DateTime AppealDeadline { get; set; }
        public bool IsAppealWindowOpen { get; set; }  // computed - بتحسبها في Service
        public int AppealsCount { get; set; }         // كام استئناف اتعمل

        // Status
        public bool IsFinalVerdict { get; set; }

        // Relations (Lightweight - مش full objects)
        public int CaseId { get; set; }
        public int CourtSessionId { get; set; }
        public DateTime CourtSessionDate { get; set; } // بس التاريخ

        
    }
}
