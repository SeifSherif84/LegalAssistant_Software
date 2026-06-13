using Domain.Contracts;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CourtSession : BaseEntity<int> , ISoftDelete
    {
        public DateTime SessionDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool ReminderSent { get; set; } = false;

        public string CourtName { get; set; }
        public string CourtRoom { get; set; }
        public string Floor { get; set; }

        public string JudgeName { get; set; }

        public SessionType SessionType { get; set; }
        public SessionStatus SessionStatus { get; set; }

        public string? AdjournmentReason { get; set; }
        public string? CancelledReason { get; set; }

        public string? Notes { get; set; }

        public int? NextSessionId { get; set; }
        public virtual CourtSession? NextSession { get; set; } // الـ Navigation Property

        public int CaseId { get; set; }
        public Case Case { get; set; }

        public List<Decision> Decisions { get; set; } = new();

        public List<Document> Documents { get; set; } = new();

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }

}
