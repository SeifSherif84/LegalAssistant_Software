using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CourtSession
    {
        public int Id { get; set; }

        public DateTime SessionDate { get; set; }

        public string Location { get; set; }

        public SessionType SessionType { get; set; }

        public int CaseId { get; set; }
        public Case Case { get; set; }

        public int? DecisionId { get; set; }
        public Decision? Decision { get; set; }

        public List<Document> Documents { get; set; } = new();

        public string? Notes { get; set; }
    }

}
