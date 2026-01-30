using AutoMapper;
using AutoMapper.Execution;
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
    public class LawyerProfilePictureSolver(IConfiguration _configuration) : IValueResolver<Lawyer, LawyerResponse, string>
    {
        public string Resolve(Lawyer source, LawyerResponse destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProfilePictureUrl))
            {
                destMember = $"{_configuration["BaseURL"]}/files/images/lawyerprofile/{source.ProfilePictureUrl}";
                return destMember;
            }
            return string.Empty;
        }
    }
}
