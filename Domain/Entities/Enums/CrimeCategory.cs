using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    // تصنيف الجريمة (مهم جداً لتحديد العقوبات والمواعيد)
    public enum CrimeCategory
    {
        Felony = 1,      // جناية
        Misdemeanor = 2, // جنحة
        Infraction = 3   // مخالفة
    }
}
