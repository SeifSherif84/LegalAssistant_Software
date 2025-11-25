using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    public enum SessionType
    {
        Hearing,     // جلسة نظر
        Appeal,      // استئناف
        Cassation,   // نقض
        Verdict,     // جلسة حكم
        Adjourned    // تأجيل
    }
}
