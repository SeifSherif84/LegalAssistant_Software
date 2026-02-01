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
            var newCase = _mapper.Map<Case>(createCaseRequest);
            newCase.CreatedAt = DateTime.UtcNow;
            newCase.UpdatedAt = DateTime.UtcNow;
            newCase.Status = CaseStatus.Open;
            var specifications = new LawyerSpecifications(LawyerId);
            var lawyer = await _unitOfWork.GetRepository<string, Lawyer>().GetByIdAsync(specifications);
            if (lawyer is null)
                throw new LawyerNotFoundException("Lawyer not found.");
            if (lawyer.Cases is null)
                lawyer.Cases = new List<Case>();
            lawyer.Cases.Add(newCase);

            await _unitOfWork.GetRepository<int, Case>().Add(newCase);
            int creationResult = await _unitOfWork.SaveChangesAsync();
            if (creationResult <= 0)
                throw new CaseCreationFailed("Failed to create case.");
        }

        public async Task<IEnumerable<CaseResponse>> GetAllCasesAsync(string LawyerId)
        {
            var lawyerSpecifications = new LawyerSpecifications(LawyerId);
            var lawyer = await _unitOfWork.GetRepository<string, Lawyer>().GetByIdAsync(lawyerSpecifications);
            if (lawyer is null)
                throw new LawyerNotFoundException("Lawyer not found.");
            var caseSpecifications = new CaseSpecifications(LawyerId);
            var cases = await _unitOfWork.GetRepository<int, Case>().GetAllAsync(caseSpecifications);
            return _mapper.Map<IEnumerable<CaseResponse>>(cases);
        }
    }
}
