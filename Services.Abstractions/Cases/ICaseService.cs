using Domain.Entities;
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
        public Task CreateCaseAsync(CreateCaseRequest createCaseRequest, string LawyerId);
        Task<IEnumerable<CaseResponse>> GetAllCasesAsync(string LawyerId);
    }
}
