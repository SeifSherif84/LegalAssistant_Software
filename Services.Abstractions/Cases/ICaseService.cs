using Domain.Entities;
using Domain.Entities.Enums;
using Shared.Dtos.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Cases
{
    public interface ICaseService
    {
        Task CreateCaseAsync(CreateCaseRequest createCaseRequest, string LawyerId);
        Task<IEnumerable<CaseResponse>> GetAllCasesAsync(string LawyerId, CaseStatus? Status, bool addedWithinMonth = false);
        Task UpdateCaseAsync(int caseId, string LawyerId, UpdateCaseRequest updateCaseRequest);
        Task DeleteCaseAsync(int caseId, string LawyerId);
        Task<CaseResponse> GetCaseByIdAsync(int caseId, string LawyerId);
    }
}
