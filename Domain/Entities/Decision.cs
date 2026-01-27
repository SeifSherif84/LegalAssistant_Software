using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Decision : BaseEntity<int>
    {
        public DateTime DecisionDate { get; set; } // تاريخ الحكم
        public DecisionType DecisionType { get; set; } // نوع الحكم (مثلاً: حكم ابتدائي، حكم استئنافي)
        public bool Appealable { get; set; } // هل الحكم قابل للاستئناف
        public string SentenceText { get; set; } // نص الحكم
        public DateTime AppealDeadline { get; set; } // الموعد النهائي للاستئناف
        public string? JudgeName { get; set; } // اسم القاضي

        public bool IsFinalVerdict { get; set; } // هل هو الحكم النهائي
        public SentenceType SentenceType { get; set; } // الحكم الصادر

        public int CourtSessionId { get; set; } 
        public CourtSession CourtSession { get; set; } // العلاقة مع جلسة المحكمة


        // For Performance: Direct FK to Case
        public int CaseId { get; set; }
        public Case Case { get; set; } // العلاقة مع القضية

    }
}
