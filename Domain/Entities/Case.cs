using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Case
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int FileNumber { get; set; }
        public string CourtName { get; set; }
        public Status Status { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public DateTime NextHearingDate { get; set; }
        public DateTime VerdictDate { get; set; }
        public string Notes { get; set; }

    }
}
