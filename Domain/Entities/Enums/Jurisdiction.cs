using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    // الاختصاص القضائي (درجة المحكمة)
    public enum Jurisdiction
    {
        جزئية = 1,
        ابتدائية = 2,
        استئناف = 3,
        نقض = 4
    }
}
