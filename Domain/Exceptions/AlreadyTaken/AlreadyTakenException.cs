using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.AlreadyTaken
{
    public class AlreadyTakenException(string message) : Exception(message)
    {
    }
}
