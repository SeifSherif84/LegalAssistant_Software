using Domain.Entities.Enums;
using Shared.Dtos.Persons;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.CaseParties
{
    public class CasePartyWithPersonRequest
    {
        //public int CaseId { get; set; }
        public PartyRole Role { get; set; }
        
        public PersonRequest Person { get; set; }

        public string? CrimeDescription { get; set; }
        public DateTime? CrimeDate { get; set; }

        public string? TestimonyText { get; set; }
        public DateTime? TestimonyDate { get; set; }

        //public string? LawyerId { get; set; }

        public string? Notes { get; set; }
    }
}
