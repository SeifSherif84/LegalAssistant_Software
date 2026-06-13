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
        جناية = 1,
        جنحة = 2,
        مخالفة = 3
    }
}
