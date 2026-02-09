using Domain.Entities;
using Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.CourtSessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.CourtSessions
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourtSessions(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet("GetSessionTypes")]
        [Authorize]
        public IActionResult GetSessionTypes() 
        {
            var sessionTypes = Enum.GetValues(typeof(SessionType))
                                     .Cast<SessionType>()
                                     .Select(ST => new
                                     {
                                         Id = (int)ST,
                                         Name = ST.ToString(),
                                     });
            return Ok(sessionTypes);
        }


        [HttpPost("AddSession/{caseId}")]
        [Authorize]
        public async Task<IActionResult> AddSession(int caseId, CreateCourtSession createCourtSession)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.CourtSessionService.AddSessionAsync(caseId, lawyerId, createCourtSession);
            return Ok("Session created successfully.");
        }


        [HttpGet("GetAllSessions/{caseId}")]
        [Authorize]
        public async Task<IActionResult> GetAllSessions(int caseId)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessions = await _serviceManager.CourtSessionService.GetAllSessionsAsync(caseId, lawyerId);
            return Ok(sessions);
        }

    }
}
