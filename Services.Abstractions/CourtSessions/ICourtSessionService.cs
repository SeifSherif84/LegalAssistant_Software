using Domain.Entities;
using Shared.Dtos.CourtSessions;
using Shared.Dtos.Dashboard;
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
        Task DeleteSessionAsync(int sessionId, string lawyerId);
        Task UpdateSessionAsync(int sessionId, string lawyerId, UpdateCourtSession updateCourtSession);
        Task<CourtSessionResponse> GetSessionByIdAsync(int sessionId, string lawyerId);
        Task<IEnumerable<CourtSessionResponseForDashboard>> GetLawyerSessionsAsync(string lawyerId, string period);
    }
}
