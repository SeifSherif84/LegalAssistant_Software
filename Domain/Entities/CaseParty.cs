using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CaseParty : BaseEntity<int>
    {
        public int CaseId { get; set; } // Foreign key to Case
        public Case Case { get; set; } // Navigation property to Case


        public int PersonId { get; set; } // Foreign key to Person
        public Person Person { get; set; } // Navigation property to Person


        // Defendant-specific
        public string? crimeDescription { get; set; }
        public DateTime? crimeDate { get; set; }


        // Witness-specific
        public string? TestimonyText { get; set; }
        public DateTime? TestimonyDate { get; set; }


        public PartyRole Role { get; set; }


        public int? LawyerId { get; set; } // Foreign key to Lawyer
        public Lawyer? Lawyer { get; set; } // Navigation property to Lawyer


        public string Notes { get; set; }

    }
}
