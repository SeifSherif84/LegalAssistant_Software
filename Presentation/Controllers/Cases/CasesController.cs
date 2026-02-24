using Domain.Entities;
using Domain.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.Cases
{

    [Route("api/[controller]")]
    [ApiController]
    public class CasesController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet("GetCrimeType")] // GET: api/Cases/GetCrimeType
        [Authorize] 
        public IActionResult GetCrimeType()
        {
            var crimeTypes = Enum.GetValues(typeof(CrimeType))
                                 .Cast<CrimeType>()
                                 .Select(ct => new
                                 {
                                     Id = (int)ct,
                                     Name = ct.ToString()
                                 });
            return Ok(crimeTypes);
        }



        [HttpGet("GetJurisdiction")] // GET: api/Cases/GetJurisdiction
        [Authorize]
        public IActionResult GetJurisdiction()
        {
            var Jurisdictions = Enum.GetValues(typeof(Jurisdiction))
                                    .Cast<Jurisdiction>()
                                    .Select(j => new
                                    {
                                        Id = (int)j,
                                        Name = j.ToString()
                                    });
            return Ok(Jurisdictions);
        }



        [HttpGet("GetCrimeCategory")] // GET: api/Cases/GetCrimeCategory
        [Authorize]
        public IActionResult GetCrimeCategory()
        {
            var CrimeCategories = Enum.GetValues(typeof(CrimeCategory))
                                    .Cast<CrimeCategory>()
                                    .Select(cc => new
                                    {
                                        Id = (int)cc,
                                        Name = cc.ToString()
                                    });
            return Ok(CrimeCategories);
        }



        [HttpGet("GetCaseStatus")] // GET: api/Cases/GetCaseStatus
        [Authorize]
        public IActionResult GetCaseStatus()
        {
            var caseStatuses = Enum.GetValues(typeof(CaseStatus))
                                    .Cast<CaseStatus>()
                                    .Select(cs => new
                                    {
                                        Id = (int)cs,
                                        Name = cs.ToString()
                                    });
            return Ok(caseStatuses);
        }



        [HttpPost("CreateCase")] // GET: api/Cases/CreateCase
        [Authorize]
        public async Task<IActionResult> CreateCase([FromBody]CreateCaseRequest createCaseRequest)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.CaseService.CreateCaseAsync(createCaseRequest, lawyerId);
            return Ok("Case created successfully.");
        }



        [HttpGet("GetAllCases")] // GET: api/Cases/GetAllCases
        [Authorize]
        public async Task<IActionResult> GetAllCases([FromQuery] CaseStatus? Status, [FromQuery] bool addedWithinMonth = false)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cases = await _serviceManager.CaseService.GetAllCasesAsync(lawyerId, Status, addedWithinMonth);
            return Ok(cases);
        }



        // [HttpPut("UpdateCase")] // PUT: api/Cases/UpdateCase?caseId={caseId} // [FromQuery]
        [HttpPut("UpdateCase/{caseId}")] // PUT: api/Cases/UpdateCase/{caseId} // [FromRoute]
        [Authorize] 
        public async Task<IActionResult> UpdateCase([FromRoute] int caseId, [FromBody] UpdateCaseRequest updateCaseRequest)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.CaseService.UpdateCaseAsync(caseId, lawyerId, updateCaseRequest);
            return Ok("Case updated successfully.");
        }



        [HttpDelete("DeleteCase/{caseId}")] // DELETE: api/Cases/DeleteCase/{caseId}
        [Authorize]
        public async Task<IActionResult> DeleteCase([FromRoute] int caseId)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.CaseService.DeleteCaseAsync(caseId, lawyerId);
            return Ok("Case deleted successfully.");
        }



        [HttpGet("GetCaseById/{caseId}")] // GET: api/Cases/GetCaseById/{caseId}
        [Authorize]
        public async Task<IActionResult> GetCaseById([FromRoute] int caseId)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var caseDetails = await _serviceManager.CaseService.GetCaseByIdAsync(caseId, lawyerId);
            return Ok(caseDetails);
        }

    }
}
