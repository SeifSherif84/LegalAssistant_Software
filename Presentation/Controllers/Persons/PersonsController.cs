using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.Persons
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddPerson(PersonRequest personRequest)
        {
            var personResponce = await _serviceManager.PersonService.CreatePerson(personRequest);
            return Ok(personResponce);
        }
        [HttpGet("{personId}")]
        public async Task<IActionResult> GetPersonByID(int personId)
        {
            var personResponce = await _serviceManager.PersonService.GetPersonByIdAsync(personId);
            return Ok(personResponce);
        }
        [HttpGet("GetAllPersons")]
        public async Task<IActionResult> GetAllPersons([FromQuery] PersonFilterDto filter)
        {
            var personResponce = await _serviceManager.PersonService.GetAllPersonsAsync(filter);
            return Ok(personResponce);
        }
        [HttpPut]
        public async Task<IActionResult> UpdatePerson(int personId, PersonRequest personRequest)
        {
            var personResponce = await _serviceManager.PersonService.UpdatePerson(personId, personRequest);
            return Ok(personResponce);
        }
        [HttpDelete]
        public async Task<IActionResult> DeletePerson(int personId)
        {
            await _serviceManager.PersonService.DeletePersonAsync(personId);
            return Ok("Person Deleted Succesfully");
        }
    }
}
