using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    public enum LawyerStatus
    {
        Pending = 0,   // لسه مستني موافقة الأدمن
        Approved = 1,  // اتوافق عليه
        Rejected = 2   // اترفض
    }

}
