using AutoMapper;
using Domain.Entities;
using Domain.Entities.HelperClass;
using Shared.Dtos.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping.Persons
{
    public class PersonProfile : Profile
    {
        public PersonProfile() 
        {
            CreateMap<ContactInfoDto, ContactInfo>()
                .ReverseMap();
                
            CreateMap<AddressDto, Address>().ReverseMap();

            CreateMap<PersonRequest, Person>()
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<Person, PersonResponce>()
                .ForMember(dest => dest.Age,
                    opt => opt.MapFrom(src =>
                        DateTime.Now < src.DateOfBirth.AddYears(DateTime.Now.Year - src.DateOfBirth.Year)
                            ? DateTime.Now.Year - src.DateOfBirth.Year - 1
                            : DateTime.Now.Year - src.DateOfBirth.Year));
        }
    }
}
