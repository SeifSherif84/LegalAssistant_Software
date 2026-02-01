using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Shared.Dtos.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Authentications
{
    public interface IAuthenticationService
    {
        Task<LawyerLoginResponse?> Login(LawyerLoginRequest loginRequest);
        Task<LawyerRegisterResponse?> Register(LawyerRegisterRequest registerRequest);
        Task<bool> ResetPasswordByEmail(ResetPasswordByEmailDto model);
        Task<IdentityResult> UpdatePassword(UpdatePasswordDto updatePasswordDto);
        Task<IdentityResult> DeleteAccountAsync(string userId);
    }
}
