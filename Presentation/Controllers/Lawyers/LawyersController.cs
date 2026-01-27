using Domain.Contracts;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
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

        [HttpGet("GetLawyerInfo")]
        [Authorize]
        public async Task<IActionResult> GetLawyerInfo()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier);
            if (id is null)
                return BadRequest("id claim not found.");

            var lawyer = await _serviceManager.LawyerService.GetLawyerInfo(id.Value);
            return Ok(lawyer);
        }
    }
}
