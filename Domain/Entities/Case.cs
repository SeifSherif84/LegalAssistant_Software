using Domain.Contracts;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Case : BaseEntity<int>, ISoftDelete
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string FileNumber { get; set; }
        public string CourtName { get; set; }
        public CaseStatus Status { get; set; }
        public string? Notes { get; set; }
        public Jurisdiction Jurisdiction { get; set; }
        public CrimeCategory CrimeCategory { get; set; }
        public CrimeType crimeType { get; set; }
        public string ClientName { get; set; }


        public List<Document> Documents { get; set; } // One-to-Many relationship with Document
        public List<CourtSession> CourtSessions { get; set; } // One-to-Many relationship with CourtSession
        public List<Lawyer> Lawyers { get; set; } // Many-to-Many relationship with Lawyer
        public List<CaseParty> CaseParties { get; set; } // One-to-Many relationship with CaseParty

        public List<AiAnalysis> AiAnalyses { get; set; } // One-to-Many relationship with AiAnalysis


        // For performance optimization
        public List<Decision> Decisions { get; set; } // One-to-Many relationship with Decision

        // For performance optimization
        public List<Appeal> Appeals { get; set; } // One-to-Many relationship with Appeal





        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
