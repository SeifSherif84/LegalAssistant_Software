using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Case : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int FileNumber { get; set; }
        public string CourtName { get; set; }
        public CaseStatus Status { get; set; }
        public DateTime? NextHearingDate { get; set; }
        public DateTime? VerdictDate { get; set; }
        public string Notes { get; set; }


        public List<Lawyer> Lawyers { get; set; } // Many-to-Many relationship with Lawyer

        public List<CaseParty> CaseParties { get; set; } // One-to-Many relationship with CaseParty


    }
}
