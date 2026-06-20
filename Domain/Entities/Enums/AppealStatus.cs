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
        UnderReview = 1,  // قيد الدراسة من النيابة أو المحكمة
        Closed = 2 // صدر قرار نهائي في الأستئناف (سواء بالقبول أو الرفض)
    }

}
