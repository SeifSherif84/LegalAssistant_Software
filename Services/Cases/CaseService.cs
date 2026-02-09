using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Exceptions.BadRequest;
using Domain.Exceptions.NotFound;
using Services.Abstractions.Cases;
using Services.Specifications.Cases;
using Services.Specifications.Lawyers;
using Shared.Dtos.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Cases
{
    public class CaseService(IMapper _mapper,
                             IUnitOfWork _unitOfWork) : ICaseService
    {
        public async Task CreateCaseAsync(CreateCaseRequest createCaseRequest, string LawyerId)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(LawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");
            var specifications = new LawyerSpecifications(LawyerId);
            var lawyer = await _unitOfWork.GetRepository<string, Lawyer>().GetByIdAsync(specifications);
            if (lawyer is null)
                throw new LawyerNotFoundException("Lawyer not found.");

            var newCase = _mapper.Map<Case>(createCaseRequest);
            newCase.CreatedAt = DateTime.UtcNow;
            newCase.UpdatedAt = DateTime.UtcNow;
            newCase.Status = CaseStatus.Open;

            if (lawyer.Cases is null)
                lawyer.Cases = new List<Case>();
            lawyer.Cases.Add(newCase);

            await _unitOfWork.GetRepository<int, Case>().Add(newCase);
            int creationResult = await _unitOfWork.SaveChangesAsync();
            if (creationResult <= 0)
                throw new CaseCreationFailedException("Failed to create case. Please try again later.");
        }


        public async Task<IEnumerable<CaseResponse>> GetAllCasesAsync(string LawyerId)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(LawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            var lawyerSpecifications = new LawyerSpecifications(LawyerId);
            var lawyer = await _unitOfWork.GetRepository<string, Lawyer>().GetByIdAsync(lawyerSpecifications);
            if (lawyer is null)
                throw new LawyerNotFoundException("Lawyer not found.");
            var caseSpecifications = new CaseSpecifications(LawyerId);
            var cases = await _unitOfWork.GetRepository<int, Case>().GetAllAsync(caseSpecifications);
            return _mapper.Map<IEnumerable<CaseResponse>>(cases);
        }


        public async Task UpdateCaseAsync(int caseId, string LawyerId, UpdateCaseRequest updateCaseRequest)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(LawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            var lawyerSpecifications = new LawyerSpecifications(LawyerId);
            var lawyer = await _unitOfWork.GetRepository<string, Lawyer>().GetByIdAsync(lawyerSpecifications);
            if (lawyer is null)
                throw new LawyerNotFoundException("Lawyer not found.");


            // 1. Check CaseId Existence
            var caseSpecifications = new CaseSpecifications(caseId, false,
                                                                    false,
                                                                    false,
                                                                    false,
                                                                    false,
                                                                    true,
                                                                    false);
            var caseEntity = await _unitOfWork.GetRepository<int, Case>().GetByIdAsync(caseSpecifications);
            if (caseEntity is null)
                throw new CaseNotFoundException($"Case with id : {caseId} not found.");


            // 2. Check if the case belongs to this lawyer
            if (caseEntity.Lawyers.Any(L => L.Id == lawyer.Id))
            {
                _mapper.Map(updateCaseRequest, caseEntity);
                caseEntity.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.GetRepository<int, Case>().Update(caseEntity);
                int result = await _unitOfWork.SaveChangesAsync();
                if (result <= 0)
                    throw new CaseUpdateFailedException("Failed to update case. Please try again later.");
            }
            else
                throw new UnauthorizedAccessException("You don't have permission to update this case.");
        }


        public async Task DeleteCaseAsync(int caseId, string LawyerId)
        {
            // 2. Check LawyerId
            if (string.IsNullOrEmpty(LawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            // 1. Check CaseId Existence
            var caseSpecifications = new CaseSpecifications(caseId, false,
                                                                    false,
                                                                    false,
                                                                    false,
                                                                    false,
                                                                    true,
                                                                    false);

            var caseEntity = await _unitOfWork.GetRepository<int, Case>().GetByIdAsync(caseSpecifications);
            if (caseEntity is null)
                throw new CaseNotFoundException($"Case with id : {caseId} not found.");

            // 3. Check if the case belongs to this lawyer
            if (caseEntity.Lawyers.Any(L => L.Id == LawyerId))
            {
                caseEntity.IsDeleted = true;
                caseEntity.DeletedAt = DateTime.UtcNow;
                int result = await _unitOfWork.SaveChangesAsync();
                if (result <= 0)
                    throw new CaseDeletionFailedException("Failed to delete case. Please try again later.");
            }
            else
                throw new UnauthorizedAccessException("You don't have permission to delete this case.");
        }






    }
}
