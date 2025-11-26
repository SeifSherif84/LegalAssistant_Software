using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appeal : BaseEntity<int>
    {
        public DateTime FiledDate { get; set; }
        public string AppealType { get; set; }
        public string Status { get; set; }
        public int CaseId { get; set; }
        public Case Case { get; set; }
    }
}
