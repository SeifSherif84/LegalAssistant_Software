using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Shared.Dtos.Lawyers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping.Lawyers
{
    public class LawyerProfile : Profile
    {
        public LawyerProfile(IConfiguration _configuration)
        {
            CreateMap<Lawyer, LawyerResponse>()
                .ForMember(D => D.ProfilePictureUrl, C => C.MapFrom(new LawyerProfilePictureSolver(_configuration)));

            CreateMap<LawyerUpdateRequest, Lawyer>();
            CreateMap<LawyerUpdateProfilePictureRequest, Lawyer>();
        }
    }
}
