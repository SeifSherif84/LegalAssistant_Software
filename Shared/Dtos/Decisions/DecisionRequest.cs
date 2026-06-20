using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Decisions
{
    public class DecisionRequest
    {
        //[Required]
        public DateTime? NextSessionDate { get; set; }

        [Required]
        public DecisionType DecisionType { get; set; }

        [Required]
        public SentenceType SentenceType { get; set; }

        [Required]
        [MaxLength(5000)]
        public string SentenceText { get; set; }

        public bool Appealable { get; set; } 

        public DateTime? AppealDeadline { get; set; }  // nullable — only required if Appealable = true

        //[MaxLength(200)]
        //public string? JudgeName { get; set; }

        public bool IsFinalVerdict { get; set; }

        //[Required]
        //public int CourtSessionId { get; set; }
        
    }
}
