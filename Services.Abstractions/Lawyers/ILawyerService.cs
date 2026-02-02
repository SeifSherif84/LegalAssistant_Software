using Domain.Entities;
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
        Task<LawyerResponse> GetLawyerInfo(string lawyerId);
        Task Update(string lawyerId, LawyerUpdateRequest lawyerUpdateRequest);
        Task UpdateProfilePicture(string lawyerId, LawyerUpdateProfilePictureRequest lawyerUpdateProfilePictureRequest);
    }
}
