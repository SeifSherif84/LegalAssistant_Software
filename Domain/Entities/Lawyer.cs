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
    }
}
