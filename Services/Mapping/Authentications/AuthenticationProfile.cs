using AutoMapper;
using Domain.Entities;
using Shared.Dtos.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping.Authentications
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<Lawyer, LawyerRegisterRequest>().ReverseMap();
        }
    }
}
