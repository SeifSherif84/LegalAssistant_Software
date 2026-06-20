using Domain.Entities;
using Shared.Dtos.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Persons
{
    public class PersonWithDetailsSpecification : BaseSpecifications<int, Person>
    {
        public PersonWithDetailsSpecification(int id) 
        {
            Criteria = P => P.Id == id;
        }
        public PersonWithDetailsSpecification(PersonFilterDto filter)
        {
            Criteria = p =>
             (string.IsNullOrEmpty(filter.FullName) || p.FullName.Contains(filter.FullName)) &&
             (string.IsNullOrEmpty(filter.Email) || p.Email.Contains(filter.Email)) &&
             (string.IsNullOrEmpty(filter.PhoneNumber) || p.PhoneNumber.Contains(filter.PhoneNumber)) &&
             (string.IsNullOrEmpty(filter.NationalIdNumber) || p.NationalIdNumber == filter.NationalIdNumber) &&
             (!filter.Gender.HasValue || p.Gender == filter.Gender) &&
             (string.IsNullOrEmpty(filter.City) || p.Address.City.Contains(filter.City)) &&
             (!filter.MinAge.HasValue || p.DateOfBirth <= DateTime.Now.AddYears(-filter.MinAge.Value)) &&
             (!filter.MaxAge.HasValue || p.DateOfBirth >= DateTime.Now.AddYears(-filter.MaxAge.Value));

            ApplyPaging(
                (filter.PageIndex - 1) * filter.PageSize,
                filter.PageSize
            );
        }
    }
}
