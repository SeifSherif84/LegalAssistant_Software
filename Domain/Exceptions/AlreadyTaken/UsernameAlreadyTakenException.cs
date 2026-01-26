using Domain.Exceptions.AlreadyTaken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.AlreadyExist
{
    public class UsernameAlreadyTakenException(string username) : 
        AlreadyTakenException($"Username : {username} Is Already Taken !")
    {
    }
}
