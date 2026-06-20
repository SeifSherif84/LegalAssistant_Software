using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.ServerError
{
    public class ServerErrorException(string Message) : Exception(Message)
    {
    }
}
