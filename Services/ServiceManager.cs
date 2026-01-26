using AutoMapper;
using Company.PL.Helper.MailKitFeature;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services.Abstractions;
using Services.Abstractions.Authentications;
using Services.Authentications;
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
                                IMailService _mailService) : IServiceManager
    {
        public IAuthenticationService AuthenticationService { get; } = new AuthenticationService(_userManager, _JWTOptions, _mapper, _mailService);
    }
}
