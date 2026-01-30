using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AiAnalysis : BaseEntity<int>
    {
        public int CaseId { get; set; }
        public Case Case { get; set; }

        public int? DocumentId { get; set; }
        public Document? Document { get; set; }

        public string Vulnerabilities { get; set; }
        public ConfidenceLevel ConfidenceLevel { get; set; }
        public AnalysisType AnalysisType { get; set; }

        public DateTime AnalyzedAt { get; set; }
    }

}
