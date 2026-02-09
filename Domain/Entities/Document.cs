using Domain.Contracts;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Document : BaseEntity<int>, ISoftDelete
    {
        public string Title { get; set; }
        public DocumentType Type { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }

        public DocumentStatus Status { get; set; } 

        public bool IsAnalyzedByAI { get; set; }
        public DateTime? AnalyzedAt { get; set; }


        public string LawyerId { get; set; }
        public Lawyer Lawyer { get; set; } // المحامي اللي رفع المستند


        public int CaseId { get; set; }
        public Case Case { get; set; }


        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
