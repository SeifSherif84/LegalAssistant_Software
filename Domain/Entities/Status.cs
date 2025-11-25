using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public enum Status
    {
        Open = 1,
        Closed = 2,
        Pending = 3,
        OnHold = 4,
        Dismissed = 5,
        Settled = 6
    }
}
