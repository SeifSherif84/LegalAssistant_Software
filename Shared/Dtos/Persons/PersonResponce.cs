using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Persons
{
    public class PersonResponce
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string NationalIdNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public int Age { get; set; } // محسوبة

        public Gender Gender { get; set; }

        public AddressDto Address { get; set; }

        public ContactInfoDto ContactInfo { get; set; }
    }
}
