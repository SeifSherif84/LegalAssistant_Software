using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.ServerError
{
    public class ServerErrorExceptionText(string Message) : ServerErrorException(Message)
    {
    }
}
