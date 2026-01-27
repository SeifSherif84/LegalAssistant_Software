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
        Preliminary = 1,    // حكم تمهيدي (مثل إحالة للخبراء أو الطب الشرعي)
        Final = 2,          // حكم قطعي (يفصل في موضوع الدعوى)
        Interlocutory = 3,  // حكم فرعي (أثناء سير القضية)
        Injunction = 4      // أمر وقتي أو مستعجل
    }
}
