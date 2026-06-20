using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.CaseParties
{
    public class CasePartyFilterDto
    {
        public int? PersonId { get; set; }
        public PartyRole? Role { get; set; }
    }
}
