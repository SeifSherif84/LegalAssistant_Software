using Domain.Entities.Identity;
using Domain.Exceptions.NotFound;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.Authentications
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController(IServiceManager _serviceManager) : ControllerBase
    {
        
        [HttpPost("login")] // POST: api/authentication/login
        public async Task<IActionResult> Login([FromBody] LawyerLoginRequest loginRequest)
        {
            var loginResponse = await _serviceManager.AuthenticationService.Login(loginRequest);
            return Ok(loginResponse);
        }


        [HttpPost("register")] // POST: api/authentication/register
        public async Task<IActionResult> Register([FromForm] LawyerRegisterRequest registerRequest)
        {
            var registerResponse = await _serviceManager.AuthenticationService.Register(registerRequest);
            return Ok(registerResponse);
        }


        [HttpPost("ResetPassword")] // POST: api/authentication/ResetPassword
        public async Task<IActionResult> ResetPasswordByEmailAsync([FromBody] ResetPasswordByEmailDto model)
        {
            await _serviceManager.AuthenticationService.ResetPasswordByEmail(model);
            return Ok("Password reset email sent successfully.");
        }


        [HttpPost("UpdatePassword")] // POST: api/authentication/UpdatePassword
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDto updatePasswordDto) 
        {
            await _serviceManager.AuthenticationService.UpdatePassword(updatePasswordDto);
            return Ok("Password updated successfully.");
        }

        [Authorize]
        [HttpPost("ChangePassword")] // POST: api/authentication/ChangePassword
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.AuthenticationService.ChangePassword(userId, model);
            return Ok("Password changed successfully.");
        }

        [Authorize]
        [HttpDelete("DeleteAccount")] // DELETE: api/authentication/DeleteAccount
        public async Task<IActionResult> DeleteAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.AuthenticationService.DeleteAccountAsync(userId);
            return Ok("Account deleted successfully.");
        }

    }
}
