using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    // نوع القرار القضائي
    public enum DecisionType
    {
        تمهيدي = 1,
        نهائي = 2,
        فرعي = 3,
        أمر_وقتي = 4
    }
}
