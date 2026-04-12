using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions.BadRequest;
using Domain.Exceptions.NotFound;
using MediatR;
using Microsoft.VisualBasic;
using Services.Abstractions.Persons;
using Services.Specifications.Persons;
using Shared.Dtos.Persons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Services.Persons
{
    public class PersonService(IUnitOfWork _unitOfWork, IMapper _mapper) : IPersonService
    {
        public async Task<PersonResponce> CreatePerson(PersonRequest personRequest)
        {
            if (personRequest is null)
                throw new BadRequestException("Request cannot be null");

            personRequest.Email = personRequest.Email.ToLower();

            var emailExistSpec = new PersonByEmailSpecification(personRequest.Email.ToLower(),null);
            var existingPerson = await _unitOfWork.GetRepository<int, Person>().AnyAsync(emailExistSpec);
            if (existingPerson)
                throw new BadRequestException("Email already exists");

            var person = _mapper.Map<Person>(personRequest);
            await _unitOfWork.GetRepository<int, Person>().Add(person);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new CreationFailedException("Failed to create Person");

            return _mapper.Map<PersonResponce>(person);
        }

        public async Task<PersonResponce> GetPersonByIdAsync(int personId)
        {
            var spec = new PersonWithDetailsSpecification(personId);
            var person = await _unitOfWork.GetRepository<int, Person>().GetByIdAsync(spec);
            if(person is null)
                throw new PersonNotFoundException("No person found");
            return _mapper.Map<PersonResponce>(person);
        }

        public async Task<IEnumerable<PersonResponce>> GetAllPersonsAsync(PersonFilterDto filter)
        {
            var spec = new PersonWithDetailsSpecification(filter);
            var persons = await _unitOfWork.GetRepository<int, Person>().GetAllAsync(spec);

            if (persons is null || persons.Count() == 0)
                throw new PersonNotFoundException("No persons found with the provided filter criteria.");

            return _mapper.Map<IEnumerable<PersonResponce>>(persons);
        }

        public async Task<PersonResponce> UpdatePerson(int personId, PersonRequest personRequest)
        {
            if (personRequest is null)
                throw new BadRequestException("Request cannot be null");

            var spec = new PersonWithDetailsSpecification(personId);
            var person = await _unitOfWork.GetRepository<int, Person>().GetByIdAsync(spec);
            if(person is null)
                throw new PersonNotFoundException("No person found");

            var email = personRequest.Email.Trim().ToLower();
            var emailExistSpec = new PersonByEmailSpecification(email, personId);
            var existingPersonWithEmail = await _unitOfWork.GetRepository<int, Person>().AnyAsync(emailExistSpec);
            if (existingPersonWithEmail)
                throw new BadRequestException("Email already exists");

            _mapper.Map(personRequest,person);

            _unitOfWork.GetRepository<int, Person>().Update(person);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new UpdateFailedBadRequestException("Failed to update Person");
            return _mapper.Map<PersonResponce>(person);
        }

        public async Task DeletePersonAsync(int personId)
        {
            var spec = new PersonWithDetailsSpecification(personId);
            var person = await _unitOfWork.GetRepository<int, Person>().GetByIdAsync(spec);
            if(person is null)
                throw new PersonNotFoundException("No person found");
            if (person.IsDeleted)
                throw new BadRequestException("Person is already deleted");
            
            person.IsDeleted = true;
            person.DeletedAt = DateTime.UtcNow;
            _unitOfWork.GetRepository<int, Person>().Update(person);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
