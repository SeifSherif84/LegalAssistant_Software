using Domain.Entities;
using Shared.Dtos.Dashboard;
using Shared.Dtos.Lawyers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Lawyers
{
    public interface ILawyerService
    {
        Task<LawyerResponse> GetLawyerInfoAsync(string lawyerId);
        Task UpdateAsync(string lawyerId, LawyerUpdateRequest lawyerUpdateRequest);
        Task UpdateProfilePictureAsync(string lawyerId, LawyerUpdateProfilePictureRequest lawyerUpdateProfilePictureRequest);
        Task<DashboardResponse> MyDashboardAsync(string lawyerId);
        Task<IEnumerable<DecisionResponseForDashboard>> GetDecisionsWithAppealDeadlineThisWeekAsync(string lawyerId);
    }
}
