using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    // نوع التحليل الذي قام به الـ AI
    public enum AnalysisType
    {
        RiskAssessment = 1,      // تقييم المخاطر (نقاط الضعف)
        DocumentSummary = 2,    // تلخيص مستند
        LegalOpinion = 3,       // رأي قانوني مبدئي
        EvidenceAnalysis = 4    // تحليل الأدلة
    }
}
