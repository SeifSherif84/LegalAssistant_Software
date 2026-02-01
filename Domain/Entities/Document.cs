using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Document : BaseEntity<int>
    {
        public string Title { get; set; }
        public DocumentType Type { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }

        public DocumentStatus Status { get; set; } 

        public bool IsAnalyzedByAI { get; set; }
        public DateTime? AnalyzedAt { get; set; }

        //public string? ExtractedTextPath { get; set; } // النص اللي الـ AI اشتغل عليه

        public string LawyerId { get; set; }
        public Lawyer Lawyer { get; set; } // المحامي اللي رفع المستند


        public int CaseId { get; set; }
        public Case Case { get; set; }
    }
}
