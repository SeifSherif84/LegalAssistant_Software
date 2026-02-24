using Domain.Entities.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Documents
{
    public class DocumentResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DocumentType Type { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedAt { get; set; }
        public DocumentStatus Status { get; set; }
        public bool IsAnalyzedByAI { get; set; }
        public DateTime? AnalyzedAt { get; set; }

    }
}
