using Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.Authentications
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialtiesController : ControllerBase
    {

        [HttpGet] // GET api/specialties
        public IActionResult GetSpecialties()
        {
            var specialties = new List<object>();
            foreach(var specialty in Enum.GetValues(typeof(Specialties)))
            {
                specialties.Add(new
                {
                    Id = (int)specialty,
                    Name = specialty.ToString()
                });
            }
            return Ok(specialties);
        }

    }
}
