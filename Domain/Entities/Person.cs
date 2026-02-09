using Domain.Contracts;
using Domain.Entities.Enums;
using Domain.Entities.HelperClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Person : BaseEntity<int> , ISoftDelete
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; } 
        public string NationalIdNumber { get; set; }    
        public ContactInfo ContactInfo { get; set; } 
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }
    }
}
