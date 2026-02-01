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
        [HttpGet("GetCrimeType")]
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


        [HttpGet("GetJurisdiction")]
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


        [HttpGet("GetCrimeCategory")]
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


        [HttpPost("CreateCase")]
        [Authorize]
        public async Task<IActionResult> CreateCase([FromBody]CreateCaseRequest createCaseRequest)
        {
            var lawyerId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (lawyerId is null)
                return BadRequest("id claim not found.");

            await _serviceManager.CaseService.CreateCaseAsync(createCaseRequest, lawyerId.Value);
            return Ok("Case created successfully.");
        }


        [HttpGet("GetAllCases")] // api/Cases/GetAllCases
        [Authorize]
        public async Task<IActionResult> GetAllCases()
        {
            var lawyerId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (lawyerId is null)
                return BadRequest("id claim not found.");
            var cases = await _serviceManager.CaseService.GetAllCasesAsync(lawyerId.Value);
            return Ok(cases);
        }


    }
}
