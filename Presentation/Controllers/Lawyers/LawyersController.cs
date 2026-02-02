using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Lawyers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.Lawyers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LawyersController(IServiceManager _serviceManager) : ControllerBase
    {
        // GET: api/lawyers/GetLawyerInfo
        [HttpGet("GetLawyerInfo")]
        [Authorize]
        public async Task<IActionResult> GetLawyerInfo()
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var lawyerResponse = await _serviceManager.LawyerService.GetLawyerInfo(lawyerId);
            return Ok(lawyerResponse);
        }


        // POST: api/lawyers/UpdateInfo
        [HttpPost("UpdateInfo")]
        [Authorize]
        public async Task<IActionResult> UpdateInfo([FromBody] LawyerUpdateRequest lawyerUpdateRequest)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.LawyerService.Update(lawyerId, lawyerUpdateRequest);
            return Ok("Lawyer info updated successfully.");
        }


        // POST: api/lawyers/UpdateProfilePicture
        [HttpPost("UpdateProfilePicture")]
        [Authorize]
        public async Task<IActionResult> UpdateProfilePicture([FromForm] LawyerUpdateProfilePictureRequest lawyerUpdateProfilePictureRequest)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.LawyerService.UpdateProfilePicture(lawyerId, lawyerUpdateProfilePictureRequest);
            return Ok("Profile picture updated successfully.");
        }

    }
}
