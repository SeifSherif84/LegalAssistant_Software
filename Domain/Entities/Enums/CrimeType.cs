using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    // نوع الجريمة الجنائية (للتصنيف والبحث)
    public enum CrimeType
    {
        Homicide = 1,    // قتل
        Theft = 2,       // سرقة
        DrugOffense = 3, // مخدرات
        Assault = 4,     // اعتداء / ضرب
        Fraud = 5,       // نصب واحتيال
        Forgery = 6      // تزوير
    }
}
