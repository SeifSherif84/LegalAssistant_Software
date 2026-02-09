using Domain.Entities;
using Shared.Dtos.CourtSessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.CourtSessions
{
    public interface ICourtSessionService
    {
        Task AddSessionAsync(int caseId, string lawyerId, CreateCourtSession createCourtSession);
        Task<IEnumerable<CourtSessionResponse>> GetAllSessionsAsync(int caseId, string lawyerId);
    }
}
