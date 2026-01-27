using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Lawyers
{
    public interface ILawyerService
    {
        Task<Lawyer> GetLawyerInfo(string lawyerId);
    }
}
