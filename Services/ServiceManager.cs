using AutoMapper;
using Company.PL.Helper.MailKitFeature;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using Services.Abstractions.Authentications;
using Services.Abstractions.Lawyers;
using Services.Authentications;
using Services.Lawyers;
using Shared.Dtos.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager(UserManager<UserApp> _userManager,
                                IOptions<JWTOptions> _JWTOptions,
                                IMapper _mapper,
                                IMailService _mailService,
                                IUnitOfWork _unitOfWork) : IServiceManager
    {
        public IAuthenticationService AuthenticationService { get; } = new AuthenticationService(_userManager, _JWTOptions, _mapper, _mailService);
        public ILawyerService LawyerService => new LawyerService(_unitOfWork, _mapper);
    }
}
