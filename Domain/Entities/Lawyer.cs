using Domain.Entities.Enums;
using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Lawyer : UserApp
    {
        public int YearsOfExperience { get; set; }
        public string BarRegistrationNumber { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string BarIdCardUrl { get; set; }   

        public LawyerStatus LawyerStatus { get; set; } = LawyerStatus.Pending; // Pending, Approved, Rejected

        public List<Case> Cases { get; set; } // Many-to-Many relationship with Case

        public int? OfficeId { get; set; }
        public Office? Office { get; set; } // Many-to-One relationship with Office



        // public decimal HourlyRate { get; set; }
        // public Specialties Specialty { get; set; }

    }
}
