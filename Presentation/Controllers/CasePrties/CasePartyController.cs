using Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.CaseParties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.CasePrties
{
    [ApiController]
    [Route("api/[controller]")]
    public class CasePartyController(IServiceManager _serviceManager) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        [Route("{caseId}")]
        public async Task<IActionResult> AddCaseParty(int caseId, CasePartyWithPersonRequest casePartyRequest)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var casePartyyResponce = await _serviceManager.CasePartyService.CreateCasePartyAsync(lawyerId, caseId, casePartyRequest);
            return Ok(casePartyyResponce);
        }
        [Authorize]
        [HttpPut]
        [Route("{caseId}/{casePartyId}")]
        public async Task<IActionResult> UpdateCaseParty(int caseId, int casePartyId, CasePartyWithPersonRequest casePartyRequest)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var casePartyyResponce = await _serviceManager.CasePartyService.UpdateCasePartyAsync(lawyerId, caseId, casePartyId, casePartyRequest);
            return Ok(casePartyyResponce);
        }
        [Authorize]
        [HttpGet]
        [Route("{caseId}")]
        public async Task<IActionResult> GetCaseParties(int caseId, [FromQuery] CasePartyFilterDto filter)
        {
            var caseParties = await _serviceManager.CasePartyService.GetCasePartiesAsync(caseId, filter);
            return Ok(caseParties);
        }
        [Authorize]
        [HttpGet]
        [Route("GetPersons")]
        public async Task<IActionResult> GetPersons()
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var persons = await _serviceManager.CasePartyService.GetPersonsAsync(lawyerId);
            return Ok(persons);
        }
        [Authorize]
        [HttpGet]
        [Route("{caseId}/{casePartyId}")]
        public async Task<IActionResult> GetCasePartyById(int caseId, int casePartyId)
        {
            var caseParty = await _serviceManager.CasePartyService.GetCasePartyByIdAsync(caseId, casePartyId);
            return Ok(caseParty);
        }
        [Authorize]
        [HttpDelete]
        [Route("{caseId}/{casePartyId}")]
        public async Task<IActionResult> DeleteCaseParty(int caseId, int casePartyId)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.CasePartyService.DeleteCasePartyAsync(lawyerId, caseId, casePartyId);
            return Ok("Case Party Deleted Succesfully");
        }
        [HttpGet("Roles")]
        //[Authorize]
        public IActionResult GetSentenceTypes()
        {
            var Roles = Enum.GetValues(typeof(PartyRole))
                                     .Cast<PartyRole>()
                                     .Select(DT => new
                                     {
                                         Id = (int)DT,
                                         Name = DT.ToString(),
                                     });
            return Ok(Roles);
        }
    }
}
