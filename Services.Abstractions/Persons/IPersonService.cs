using Domain.Entities;
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
        Task<PersonResponse> CreatePerson(PersonRequest personRequest);
        Task<PersonResponse> UpdatePerson(int personId, PersonRequest personRequest);
        Task<PersonResponse> GetPersonByIdAsync(int personId);
        Task<Person> GetPersonByNationalIdAsync(string nationalId);
        Task<IEnumerable<PersonResponse>> GetAllPersonsAsync(PersonFilterDto filter);
        Task DeletePersonAsync(int personId);
    }
}
