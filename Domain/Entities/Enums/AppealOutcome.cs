using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    public enum AppealOutcome
    {
        Pending = 1,             // لم يصدر قرار بعد
        Upheld = 2,              // تأييد الحكم الأصلي (خسارة الاستئناف)
        Overturned = 3,          // إلغاء الحكم (براءة أو بطلان)
        SentenceReduced = 4,     // تخفيف الحكم (مثلاً من 5 سنين لسنة)
        SentenceIncreased = 5,   // تشديد الحكم (لو النيابة هي اللي استأنفت)
        SentBack = 6             // إعادة المحاكمة أمام دائرة أخرى
    }
}
