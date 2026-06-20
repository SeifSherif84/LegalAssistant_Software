using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BadRequest
{
    public class RegisterationBadRequestException(IEnumerable<string> Errors) : 
        BadRequestException(string.Join(", ", Errors))
    {
    }
}
