using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Lawyer : Person<int>
    {
        public Specialties Specialty { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal HourlyRate { get; set; }


        public List<Case> Cases { get; set; } // Many-to-Many relationship with Case


        public int OfficeId { get; set; }
        public Office Office { get; set; } // Many-to-One relationship with Office
    }
}
