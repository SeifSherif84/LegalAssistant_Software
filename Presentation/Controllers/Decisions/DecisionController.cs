using Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Decisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.Decisions
{
    [ApiController]
    [Route("api/[controller]")]
    public class DecisionController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost("sessions/{sessionId}")]
        [Authorize]
        public async Task<IActionResult> CreateDecisionAsync(int sessionId, DecisionRequest request)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.DecisionService.CreateDecision(lawyerId, sessionId, request);
            return Ok("Decision created successfully.");
        }

        [HttpGet("cases/{caseId}/decisions/{decisionId}")]
        [Authorize]
        public async Task<IActionResult> GetDecisionByIdAsync(int caseId,int decisionId)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var decision = await _serviceManager.DecisionService.GetDecisionByIdAsync(caseId, decisionId, lawyerId);
            return Ok(decision);
        }

        [HttpGet("cases/{caseId}/GetAllDecisions")]
        [Authorize]
        public async Task<IActionResult> GetAllDecisionsAsync(int caseId,[FromQuery] DecisionFilterDto filter)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var decisions = await _serviceManager.DecisionService.GetAllDecisionsAsync(caseId, lawyerId, filter);
            return Ok(decisions);
        }

        [HttpPut("cases/{caseId}/decisions/{decisionId}")]
        [Authorize]
        public async Task<IActionResult> UpdateDecisionAsync(int caseId, int decisionId, DecisionRequest request)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.DecisionService.UpdateDecision(caseId, decisionId, lawyerId, request);
            return Ok("Decision updated successfully.");
        }

        [HttpDelete("cases/{caseId}/decisions/{decisionId}")]
        [Authorize]
        public async Task<IActionResult> DeleteDecisionAsync(int caseId, int decisionId)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.DecisionService.DeleteDecisionAsync(caseId, decisionId, lawyerId);
            return Ok("Decision deleted successfully.");
        }

        [HttpGet("DecisionTypes")]
        [Authorize]
        public IActionResult GetDecisionTypes()
        {
            var sessionTypes = Enum.GetValues(typeof(DecisionType))
                                     .Cast<DecisionType>()
                                     .Select(DT => new
                                     {
                                         Id = (int)DT,
                                         Name = DT.ToString(),
                                     });
            return Ok(sessionTypes);
        }
    }
}
