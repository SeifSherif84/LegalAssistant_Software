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
        Summary = 1,      // محكمة جزئية
        FirstInstance = 2, // محكمة ابتدائية (كلية)
        Appellate = 3,    // محكمة استئناف
        Cassation = 4     // محكمة النقض
    }
}
