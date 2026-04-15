using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.Persons
{
    public class PersonByNationalIdSpecification : BaseSpecifications<int,Person>
    {
        public PersonByNationalIdSpecification(string nationalId, int? currentPersonId) 
        {
            Criteria = P => P.NationalIdNumber == nationalId && !P.IsDeleted && (!currentPersonId.HasValue || P.Id != currentPersonId.Value);
        }
    }
}
