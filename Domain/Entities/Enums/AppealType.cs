using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    // نوع الاستئناف - مهم جداً لتحديد المواعيد القانونية
    public enum AppealType
    {
        Appeal = 1,         // استئناف (الإجراء الطبيعي)
        Cassation = 2,      // نقض (أمام محكمة النقض)
        Opposition = 3,     // معارضة (لو الحكم كان غيابي)
        SummaryAppeal = 4   // استئناف مستعجل
    }
}
