using Shared.Dtos.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Persons
{
    public interface IPersonService
    {
        Task<PersonResponce> CreatePerson(PersonRequest personRequest);
        Task<PersonResponce> UpdatePerson(int personId, PersonRequest personRequest);
        Task<PersonResponce> GetPersonByIdAsync(int personId);
        Task<IEnumerable<PersonResponce>> GetAllPersonsAsync(PersonFilterDto filter);
        Task DeletePersonAsync(int personId);
    }
}
