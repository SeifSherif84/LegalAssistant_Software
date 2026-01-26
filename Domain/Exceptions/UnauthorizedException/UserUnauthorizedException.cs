using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.UnauthorizedException
{
    public class UserUnauthorizedException() : 
        UnauthorizedException("User Is Unauthorized !")
    {
    }
}
