using AutoMapper;
using Company.PL.Helper.MailKitFeature;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Exceptions.AlreadyExist;
using Domain.Exceptions.BadRequest;
using Domain.Exceptions.NotFound;
using Domain.Exceptions.ServerError;
using Domain.Exceptions.UnauthorizedException;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions.Authentications;
using Services.Helper;
using Shared.Dtos.Authentications;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Authentications
{
    public class AuthenticationService(UserManager<UserApp> _userManager, 
                                       IOptions<JWTOptions> _JWTOptions,
                                       IMapper _mapper,
                                       IMailService _mailService,
                                       IConfiguration _configuration) : IAuthenticationService
    {
        public async Task<LawyerLoginResponse?> Login(LawyerLoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user is null)
                throw new UserNotFoundException(loginRequest.Email);
            var flag = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!flag)
                throw new UserUnauthorizedException();
            return new LawyerLoginResponse
            {
                Email = loginRequest.Email,
                DisplayName = user.FirstName + " " + user.LastName,
                Token = await GenerateJwtTokenAsync(user)
            };
        }


        public async Task<LawyerRegisterResponse?> Register(LawyerRegisterRequest lawyerRegisterRequest)
        {
            var user = await _userManager.FindByNameAsync(lawyerRegisterRequest.UserName);
            if (user is not null)
                throw new UsernameAlreadyTakenException(lawyerRegisterRequest.UserName);

            if(lawyerRegisterRequest.ProfilePicture is not null)
            {
                lawyerRegisterRequest.ProfilePictureUrl = LawyerImageHelper.UploadProfilePicture(lawyerRegisterRequest.ProfilePicture, "lawyerprofile");
            }
            lawyerRegisterRequest.BarIdCardUrl = LawyerImageHelper.UploadBarIdCardPicture(lawyerRegisterRequest.BarIdCard, "lawyerbarIdcard");

            var lawyer = _mapper.Map<Lawyer>(lawyerRegisterRequest);
            var result = await _userManager.CreateAsync(lawyer, lawyerRegisterRequest.Password);
            if (!result.Succeeded)
                throw new RegisterationBadRequestException(result.Errors.Select(E => E.Description).ToList());

            await _userManager.AddToRoleAsync(lawyer, "Lawyer");
            return new LawyerRegisterResponse
            {
                Email = lawyerRegisterRequest.Email,
                DisplayName = lawyerRegisterRequest.FirstName + " " + lawyerRegisterRequest.LastName,
                Token = await GenerateJwtTokenAsync(lawyer)
            };
        }


        private async Task<string> GenerateJwtTokenAsync(UserApp user)
        {
            List<Claim> userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.GivenName, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var JWTOptionValues = _JWTOptions.Value;
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTOptionValues.SecurityKey));

            var token = new JwtSecurityToken(
                issuer: JWTOptionValues.Issuer,
                audience: JWTOptionValues.Audience,
                claims: userClaims,
                expires: DateTime.UtcNow.AddDays(JWTOptionValues.ExpiredDurationInDay),
                signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task ResetPasswordByEmail(ResetPasswordByEmailDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                throw new UserNotFoundException(model.Email);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = System.Web.HttpUtility.UrlEncode(token);

            var frontendUrl = _configuration["FrontendResetPasswordFormURL"];

            var callbackUrl = $"{frontendUrl}?email={model.Email}&token={encodedToken}";

            Email email = new Email()
            {
                To = model.Email,
                Subject = "Reset Your Password",
                Body = $"Please use the following token to reset your password: {callbackUrl}"
            };

            var result = _mailService.SendMail(email);
            if (!result)
                throw new ServerErrorExceptionText("Failed to send email. Please try again later.");
        }


        public async Task UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(updatePasswordDto.Email);
            if (user is null)
                throw new UserNotFoundException(updatePasswordDto.Email);

            var decodedToken = System.Web.HttpUtility.UrlDecode(updatePasswordDto.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, updatePasswordDto.NewPassword);
            if (!result.Succeeded)
                throw new ServerErrorExceptionList(result.Errors.Select(E => E.Description).ToList());
        }

        public async Task ChangePassword(string userId, ChangePasswordDto model) 
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                throw new UserNotFoundException(user.Email);

            if (model.NewPassword != model.ConfirmPassword)
                throw new PasswordsDoNotMatchBadRequestException("New password and confirm password do not match.");

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
                throw new ServerErrorExceptionList(result.Errors.Select(e => e.Description).ToList());
        }

        public async Task DeleteAccountAsync(string userId)
        {
            // 1. Check userId
            if (string.IsNullOrEmpty(userId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null || user.IsDeleted)
                throw new UserNotFoundException("User not found or already deleted.");
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new ServerErrorExceptionList(result.Errors.Select(E => E.Description).ToList());
        }

    }
}
