using AutoMapper;
using Domain.Entities;
using Shared.Dtos.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping.Cases
{
    public class CaseProfile : Profile
    {
        public CaseProfile()
        {
            CreateMap<CreateCaseRequest, Case>();
            CreateMap<Case, CaseResponse>();
            CreateMap<UpdateCaseRequest, Case>();
        }
    }
}
