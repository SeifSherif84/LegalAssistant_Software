using Domain.Contracts;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appeal : BaseEntity<int> , ISoftDelete
    {
        public DateTime? AppealDate { get; set; } // تاريخ الاستئناف
        public AppealType AppealType { get; set; } // نوع الاستئناف (مثلاً: استئناف أولي، استئناف نهائي)
        public AppealStatus Status { get; set; } // حالة الاستئناف (معلق، مقبول، مرفوض)
        public string Reason { get; set; } // سبب الاستئناف
        public string Notes { get; set; } // ملاحظات إضافية
        public AppealOutcome Outcome { get; set; } // نتيجة الاستئناف

        public int DecisionId { get; set; } 
        public Decision OriginalDecision { get; set; } // العلاقة مع القرار الأصلي


        public int? ResultDecisionId { get; set; } // القرار الجديد الناتج عن الاستئناف
        public Decision? ResultDecision { get; set; }


        // 1. من هو الطرف المستأنف؟ (المتهم أو المجني عليه)
        // نجعله Nullable لأن المستأنف قد يكون النيابة العامة (وهي ليست CaseParty)
        public int? AppealingPartyId { get; set; }
        public CaseParty? AppealingParty { get; set; }


        // 2. من المحامي الذي تولى إجراءات الاستئناف؟
        public string? LawyerId { get; set; }
        public Lawyer? Lawyer { get; set; }


        // 3. جهة الاستئناف (نيابة أم دفاع) - مهم جداً للـ AI Assistant
        public AppellantSide AppellantSide { get; set; }


        // For Performance: Direct FK to Case
        public int CaseId { get; set; }
        public Case Case { get; set; } // العلاقة مع القضية


        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
