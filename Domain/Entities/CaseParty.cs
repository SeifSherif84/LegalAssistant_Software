using Domain.Contracts;
using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CaseParty : BaseEntity<int>, ISoftDelete
    {
        public int CaseId { get; set; } // Foreign key to Case
        public Case Case { get; set; } // Navigation property to Case


        public int PersonId { get; set; } // Foreign key to Person
        public Person? Person { get; set; } // Navigation property to Person


        // Defendant-specific 
        // معلومات عن الجريمة المرتكبة لو الطرف هو متهم
        public string? crimeDescription { get; set; }
        public DateTime? crimeDate { get; set; }


        // Witness-specific
        // معلومات عن شهادة الشاهد لو الطرف هو شاهد
        public string? TestimonyText { get; set; }
        public DateTime? TestimonyDate { get; set; }


        public PartyRole Role { get; set; }

        // Lawyer information
        // المحامي اللي بيمثل الطرف في القضية
        // لو مفيش محامي، القيمة هتكون null
        public string? LawyerId { get; set; } // Foreign key to Lawyer
        public Lawyer? Lawyer { get; set; } // Navigation property to Lawyer


        public string? Notes { get; set; }


        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }

}
