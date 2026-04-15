using AutoMapper;
using Domain.Entities;
using Shared.Dtos.CaseParties;
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
        }
    }
}
