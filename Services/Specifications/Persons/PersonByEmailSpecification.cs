using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Persons
{
    public class PersonByEmailSpecification : BaseSpecifications<int,Person>
    {
        public PersonByEmailSpecification(string Email, int? currentPersonId) 
        {
            Criteria = P => P.Email == Email  && !P.IsDeleted && (!currentPersonId.HasValue || P.Id != currentPersonId.Value);
        }
    }
}
