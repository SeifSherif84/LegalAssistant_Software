using Shared.Dtos.CaseParties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.CaseParty
{
    public interface ICasePartyService
    {
        Task<CasePartyWithPersonResponse> CreateCasePartyAsync(string lawyerId,int caseId, CasePartyWithPersonRequest request);
        Task<CasePartyWithPersonResponse> UpdateCasePartyAsync(string lawyerId, int caseId, int casePartyId, CasePartyWithPersonRequest request);
        Task<CasePartyWithPersonResponse> GetCasePartyByIdAsync(int caseId, int casePartyId);
        Task<IEnumerable<CasePartyWithPersonResponse>> GetCasePartiesAsync(int caseId, CasePartyFilterDto filter);
        Task DeleteCasePartyAsync(string lawyerId, int caseId, int casePartyId);
    }
}
