using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    public enum AppellantSide
    {
        Defense = 1,      // الدفاع (المتهم أو محاميه)
        Prosecution = 2,  // النيابة العامة
        CivilClaimant = 3 // المدعي بالحق المدني
    }
}
