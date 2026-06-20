using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.ChatBot.Constants
{
    public static class ConanWarningPrefixes
    {
        /// <summary>LLM hallucinated articles — show red banner, treat answer as suspect.</summary>
        public const string HallucinatedCitation = "تنبيه: المواد التالية مذكورة في الإجابة لكنها غير موجودة في المصادر";

        /// <summary>Corrective retry repaired citations — optional info chip.</summary>
        public const string CitationCorrected = "تم تصحيح الاستشهادات";

        /// <summary>Article-lookup rescue succeeded — optional info chip.</summary>
        public const string ArticleLookupRescue = "تم استرجاع مواد إضافية من قاعدة البيانات";

        /// <summary>Iterative retrieval auto-expanded k — optional info chip.</summary>
        public const string RetrievalExpanded = "تم توسيع نطاق البحث تلقائياً";

        /// <summary>Attached document was used — info chip.</summary>
        public const string AttachmentUsed = "تم تضمين المستند المرفق";

        /// <summary>Answer is effectively a refusal — low-confidence banner.</summary>
        public const string AnswerRefusal = "النصوص المسترجعة لا تتضمن إجابة كاملة";

        /// <summary>Confidence below 0.5 — low-confidence banner.</summary>
        public const string LowConfidence = "ثقة الإجابة منخفضة";

        /// <summary>Input gate: question too vague — treat as rephrase request.</summary>
        public const string IncompleteQuestion = "السؤال غير مكتمل أو غير واضح";

        /// <summary>Fallback LLM was used — optional info chip.</summary>
        public const string FallbackModel = "ملاحظة: تعذّر الوصول إلى النموذج الأساسي";
    }
}
