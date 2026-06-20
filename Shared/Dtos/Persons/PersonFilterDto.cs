using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Persons
{
    public class PersonFilterDto
    {
        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? NationalIdNumber { get; set; }

        public Gender? Gender { get; set; }

        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }

        public string? City { get; set; }

        // Pagination
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
