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
        قتل = 1,
        سرقة = 2,
        مخدرات = 3,
        اعتداء = 4,
        احتيال = 5,
        تزوير = 6
    }
}
