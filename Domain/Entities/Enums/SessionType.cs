using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    public enum SessionType
    {
        Hearing = 1,        // جلسة نظر
        Appeal = 2,         // استئناف
        Cassation = 3,      // نقض
        Verdict = 4,        // جلسة حكم
        Adjourned = 5,      // تأجيل
        Preliminary = 6,    // أولية
        Execution = 7,      // تنفيذ
        Investigation = 8  // تحقيق
    }

}
