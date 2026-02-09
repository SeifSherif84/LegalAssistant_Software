using Domain.Entities.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.CourtSessions
{
    public class CreateCourtSession
    {
        public DateTime SessionDate { get; set; }
        public DateTime? ReminderDate { get; set; }

        public string CourtName { get; set; }
        public string CourtRoom { get; set; }
        public string Floor { get; set; } // 

        public string JudgeName { get; set; }

        public SessionType SessionType { get; set; }

        public string? Notes { get; set; }
    }
}
