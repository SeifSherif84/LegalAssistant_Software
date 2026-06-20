using Domain.Entities.Enums;
using Shared.Dtos.Lawyers;
using Shared.Dtos.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.CaseParties
{
    public class CasePartyWithPersonResponse
    {
        public int Id { get; set; }

        public int CaseId { get; set; }
        public string CaseName { get; set; }

        public PersonResponse Person { get; set; }

        public PartyRole Role { get; set; }

        public string? CrimeDescription { get; set; }
        public DateTime? CrimeDate { get; set; }

        public string? TestimonyText { get; set; }
        public DateTime? TestimonyDate { get; set; }

        public LawyerResponse? Lawyer { get; set; }

        public string? Notes { get; set; }
    }
}
