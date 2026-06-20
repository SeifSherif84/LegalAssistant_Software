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
    public class CourtSessionsController(IServiceManager _serviceManager) : ControllerBase
    {

        [HttpGet("GetSessionTypes")] // GET: api/CourtSessions/GetSessionTypes
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



        [HttpPost("AddSession/Case/{caseId}")] // POST: api/CourtSessions/AddSession/Case/{caseId}
        [Authorize]
        public async Task<IActionResult> AddSession([FromRoute] int caseId, [FromBody] CreateCourtSession createCourtSession)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.CourtSessionService.AddSessionAsync(caseId, lawyerId, createCourtSession);
            return Ok("Session created successfully.");
        }



        // Get All Sessions Related To Specific Case
        [HttpGet("GetAllSessions/Case/{caseId}")] // GET: api/CourtSessions/GetAllSessions/Case/{caseId}
        [Authorize]
        public async Task<IActionResult> GetAllSessions(int caseId)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessions = await _serviceManager.CourtSessionService.GetAllSessionsAsync(caseId, lawyerId);
            return Ok(sessions);
        }



        // For Dashboard
        // Get All Sessions For All Cases Which Related To Specific Lawyer 
        [HttpGet("GetLawyerSessions")]
        [Authorize]
        public async Task<IActionResult> GetLawyerSessions([FromQuery] string period = "all")
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sessions = await _serviceManager.CourtSessionService.GetLawyerSessionsAsync(lawyerId, period);
            return Ok(sessions);
        }



        [HttpDelete("DeleteSession/{sessionId}")] // DELETE: api/CourtSessions/DeleteSession/{sessionId}        
        [Authorize]
        public async Task<IActionResult> DeleteSession(int sessionId)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.CourtSessionService.DeleteSessionAsync(sessionId, lawyerId);
            return Ok("Session deleted successfully.");
        }




        [HttpPut("UpdateSession/{sessionId}")] // PUT: api/CourtSessions/UpdateSession/{sessionId}
        [Authorize]
        public async Task<IActionResult> UpdateSession([FromRoute] int sessionId, [FromBody] UpdateCourtSession updateCourtSession)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.CourtSessionService.UpdateSessionAsync(sessionId, lawyerId, updateCourtSession);
            return Ok("Session updated successfully.");
        }




        [HttpGet("GetSessionById/{sessionId}")] // GET: api/CourtSessions/GetSessionById/{sessionId}
        [Authorize]
        public async Task<IActionResult> GetSessionById(int sessionId)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var session = await _serviceManager.CourtSessionService.GetSessionByIdAsync(sessionId, lawyerId);
            return Ok(session);
        }

    }
}
