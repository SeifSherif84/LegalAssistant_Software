using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    public enum LawyerStatus
    {
        Pending = 1,   // لسه مستني موافقة الأدمن
        Approved = 2,  // اتوافق عليه
        Rejected = 3   // اترفض
    }

}
