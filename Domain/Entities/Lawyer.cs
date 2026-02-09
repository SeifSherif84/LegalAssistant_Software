using Domain.Contracts;
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

        public List<Document> Documents { get; set; } // One-to-Many relationship with Document

        public List<Appeal> Appeals { get; set; } // One-to-Many relationship with Appeal

    }
}
