using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Decision : BaseEntity<int>
    {
        public DateTime DecisionDate { get; set; }
        public string DecisionType { get; set; }
        public bool Appealable { get; set; }
        public string DecisionText { get; set; }
        public DateTime AppealDeadline { get; set; }

        public int CaseId { get; set; }
        public Case Case { get; set; }

    }
}
