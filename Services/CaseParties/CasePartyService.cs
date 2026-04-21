using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions.BadRequest;
using Domain.Exceptions.NotFound;
using Domain.Exceptions.UnauthorizedException;
using MediatR;
using Services.Abstractions.CaseParty;
using Services.Abstractions.Persons;
using Services.Specifications.CaseParties;
using Services.Specifications.Persons;
using Shared.Dtos.CaseParties;
using Shared.Dtos.Persons;
using Shared.Events.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CaseParties
{
    public class CasePartyService(IUnitOfWork _unitOfWork, IMapper _mapper, IMediator _mediator,IPersonService _personService) : ICasePartyService
    {
        public async Task<CasePartyWithPersonResponse> CreateCasePartyAsync(string lawyerId, int caseId, CasePartyWithPersonRequest request)
        {
            var caseEntity = await GetCase(caseId);

            EnsureLawyerAuthorized(caseEntity, lawyerId);

            var caseParty = _mapper.Map<CaseParty>(request);
            caseParty.CaseId = caseId;

            if(request.Role==Domain.Entities.Enums.PartyRole.WITNESS)
                caseParty.LawyerId = null;
            else
                caseParty.LawyerId = lawyerId;

            var existingPerson = await _personService.GetPersonByNationalIdAsync(request.Person.NationalIdNumber);
            //_mapper.Map(request.Person, existingPerson);
            
            //if (existingPerson != null && request.Person.Email != existingPerson.Email)
            //    throw new BadRequestException("The National ID provided is already registered with a different email address.");
 
            if (existingPerson != null)
            {
                if(request.Person.Email != existingPerson.Email)
                    throw new BadRequestException("The National ID provided is already registered with a different email address.");

                _mapper.Map(request.Person, existingPerson);

                caseParty.PersonId = existingPerson.Id;
                caseParty.Person = null!;
            }
            else
                caseParty.Person = _mapper.Map<Person>(request.Person);

            await _unitOfWork.GetRepository<int, CaseParty>().Add(caseParty);
            var result = await _unitOfWork.SaveChangesAsync();

            if (result <= 0)
                throw new CreationFailedException("Failed to create case party.");

            if (existingPerson != null)
                caseParty.Person = existingPerson;

            return _mapper.Map<CasePartyWithPersonResponse>(caseParty);
        }



        public async Task<IEnumerable<CasePartyWithPersonResponse>> GetCasePartiesAsync(int caseId, CasePartyFilterDto filter)
        {
            var spec = new CasePartiesWithFilterSpecification(caseId, filter.PersonId, filter.Role);
            var caseParties = await _unitOfWork.GetRepository<int, CaseParty>().GetAllAsync(spec);

            if (caseParties is null || caseParties.Count() == 0)
                throw new PersonNotFoundException("No Case Parties found with the provided filter criteria.");

            return _mapper.Map<IEnumerable<CasePartyWithPersonResponse>>(caseParties);
        }
        public async Task<IEnumerable<PersonResponse>> GetPersonsAsync(string lawyerId)
        {
            var spec = new CasePartiesWithLawyerIdSpecification(lawyerId);
            var caseParties = await _unitOfWork.GetRepository<int, CaseParty>().GetAllAsync(spec);

            if (caseParties is null || caseParties.Count() == 0)
                throw new PersonNotFoundException("No Case Parties found with the provided filter criteria.");

            return _mapper.Map<IEnumerable<PersonResponse>>(caseParties);
        }


        public async Task<CasePartyWithPersonResponse> GetCasePartyByIdAsync(int caseId, int casePartyId)
        {
            var spec = new CasePartyWithIdSpecification(caseId, casePartyId);
            var caseParty = await _unitOfWork.GetRepository<int, CaseParty>().GetByIdAsync(spec);
            if (caseParty is null)
                throw new PersonNotFoundException("No Case Party found. ");
            return _mapper.Map<CasePartyWithPersonResponse>(caseParty);
        }

        public async Task<CasePartyWithPersonResponse> UpdateCasePartyAsync(string lawyerId, int caseId, int casePartyId, CasePartyWithPersonRequest request)
        {
            var spec = new CasePartyWithIdSpecification(caseId, casePartyId);
            var caseParty = await _unitOfWork.GetRepository<int, CaseParty>().GetByIdAsync(spec);
            if (caseParty is null)
                throw new PersonNotFoundException("No Case Party found. ");

            EnsureLawyerAuthorized(caseParty.Case, lawyerId);

            _mapper.Map(request, caseParty);

            _unitOfWork.GetRepository<int, CaseParty>().Update(caseParty);
            var result = await _unitOfWork.SaveChangesAsync();

            //await _personService.UpdatePerson(caseParty.PersonId, request.Person);
            return _mapper.Map<CasePartyWithPersonResponse>(caseParty);
        }
        public async Task DeleteCasePartyAsync(string lawyerId, int caseId, int casePartyId)
        {
            var spec = new CasePartyWithCaseSpecification(caseId, casePartyId);
            var caseParty = await _unitOfWork.GetRepository<int, CaseParty>().GetByIdAsync(spec);

            if (caseParty is null)
                throw new PersonNotFoundException("No Case Party found. ");

            EnsureLawyerAuthorized(caseParty.Case, lawyerId);

            if (caseParty.IsDeleted)
                throw new BadRequestException("Case Party is already deleted");
            caseParty.IsDeleted = true;
            caseParty.DeletedAt = DateTime.UtcNow;
            _unitOfWork.GetRepository<int, CaseParty>().Update(caseParty);

            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<Case> GetCase(int caseId)
        {
            var caseResponse = await _mediator.Send(new GetCaseCommand(caseId));
            return _mapper.Map<Case>(caseResponse);
        }
        private static void EnsureLawyerAuthorized(Case caseEntity, string lawyerId)
        {
            if (!caseEntity.Lawyers.Any(l => l.Id == lawyerId))
                throw new UnauthorizedException("You are not authorized");
        }

    }
}
