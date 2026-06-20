using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Decisions
{
    public class DecisionFilterDto
    {
        //[Required]
        //public int CaseId { get; set; }
        public int? CourtSessionId { get; set; }
        public bool? IsFinal { get; set; }
    }
}
