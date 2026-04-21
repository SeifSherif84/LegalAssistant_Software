using AutoMapper;
using Domain.Entities;
using Shared.Dtos.CaseParties;
using Shared.Dtos.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping.CaseParties
{
    public class CasePartyProfile : Profile
    {
        public CasePartyProfile() 
        {
            CreateMap<CaseParty, CasePartyWithPersonResponse>()
                .ForMember(dest => dest.CaseName, opt => opt.MapFrom(src => src.Case.Title))
                .ForMember(dest => dest.Person, opt => opt.MapFrom(src => src.Person))
                .ForMember(dest => dest.Lawyer, opt => opt.MapFrom(src => src.Lawyer));
            CreateMap<CasePartyWithPersonRequest, CaseParty>();
            CreateMap<CaseParty, PersonResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Person.Id))
                .ForMember(dest => dest.ContactInfo, opt => opt.MapFrom(src => src.Person.ContactInfo))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Person.Address))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateTime.Now.Year - src.Person.DateOfBirth.Year))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Person.Gender))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Person.FullName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Person.Email))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Person.PhoneNumber))
                .ForMember(dest => dest.NationalIdNumber, opt => opt.MapFrom(src => src.Person.NationalIdNumber))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Person.DateOfBirth))
                .ForMember(dest => dest.CaseName, opt => opt.MapFrom(src => src.Case.Title))
            .ReverseMap();
        }
    }
}
