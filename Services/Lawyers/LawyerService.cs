using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions.NotFound;
using Services.Abstractions.Lawyers;
using Services.Specifications.Lawyers;
using Shared.Dtos.Lawyers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Lawyers
{
    public class LawyerService(IUnitOfWork _unitOfWork,
                               IMapper _mapper) : ILawyerService
    {
        public async Task<LawyerResponse?> GetLawyerInfo(string lawyerId)
        {
            var specifications = new LawyerSpecifications(lawyerId);
            var lawyer = await _unitOfWork.GetRepository<string, Lawyer>().GetByIdAsync(specifications);
            return _mapper.Map<LawyerResponse>(lawyer);             
        }

    }
}
