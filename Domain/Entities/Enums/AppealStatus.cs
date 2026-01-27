using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    // حالة الاستئناف من الناحية الإدارية
    public enum AppealStatus
    {
        Draft = 1,        // مسودة (المحامي بيجهزها لسه)
        Filed = 2,        // تم تقديمه للمحكمة
        UnderReview = 3,  // قيد الدراسة من النيابة أو المحكمة
        Scheduled = 4,    // تم تحديد جلسة لنظره
        Closed = 5        // انتهى تماماً
    }

}
