using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.BadRequest
{
    public class CaseUpdatedFailedException(string Message) : BadRequestException(Message)
    {
    }
}
