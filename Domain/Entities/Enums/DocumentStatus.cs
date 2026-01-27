using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    public enum DocumentStatus
    {
        Pending = 1,
        Processing = 2,
        Analyzed = 3,
        Uploaded = 4,
        Reviewed = 5,
        Rejected = 6,
        Failed = 7
    }

}
