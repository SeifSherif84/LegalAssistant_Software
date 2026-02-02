using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Exceptions.BadRequest;
using Domain.Exceptions.NotFound;
using Domain.Exceptions.ServerError;
using Services.Abstractions.Lawyers;
using Services.Helper;
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
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            var specifications = new LawyerSpecifications(lawyerId);
            var lawyer = await _unitOfWork.GetRepository<string, Lawyer>().GetByIdAsync(specifications);
            if (lawyer is null)
                throw new LawyerNotFoundException("Lawyer not found");
            return _mapper.Map<LawyerResponse>(lawyer);             
        }


        // lawyer = _mapper.Map<Lawyer>(lawyerUpdateRequest); // Wrong mapping, should map to existing lawyer entity instead of creating a new one 
        public async Task Update(string lawyerId, LawyerUpdateRequest lawyerUpdateRequest)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            var specifications = new LawyerSpecifications(lawyerId);
            var lawyer = await _unitOfWork.GetRepository<string, Lawyer>().GetByIdAsync(specifications);
            if (lawyer is null)
                throw new LawyerNotFoundException("Lawyer not found");

            _mapper.Map(lawyerUpdateRequest, lawyer); // Correct mapping to update existing entity which tracked by EF Core. Remain other properties Which Not Exist In UpdateRequest Not Be Overwritten 
            _unitOfWork.GetRepository<string, Lawyer>().Update(lawyer);
            int result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new ServerErrorExceptionText("Failed to update lawyer info. Please try again later.");
        }


        public async Task UpdateProfilePicture(string lawyerId, LawyerUpdateProfilePictureRequest lawyerUpdateProfilePictureRequest)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            var specifications = new LawyerSpecifications(lawyerId);
            var lawyer = await _unitOfWork.GetRepository<string, Lawyer>().GetByIdAsync(specifications);

            if (lawyer is null)
                throw new LawyerNotFoundException("Lawyer not found");

            if(lawyerUpdateProfilePictureRequest.ProfilePicture is not null && lawyer.ProfilePictureUrl is not null) // Mean A File Is Uploaded And There Is An Old Picture To Delete
                LawyerImageHelper.DeleteProfilePicture(lawyer.ProfilePictureUrl, "lawyerprofile");

            if (lawyerUpdateProfilePictureRequest.ProfilePicture is not null) // Mean A File Is Uploaded
                lawyerUpdateProfilePictureRequest.ProfilePictureUrl = LawyerImageHelper.UploadProfilePicture(lawyerUpdateProfilePictureRequest.ProfilePicture, "lawyerprofile");

            _mapper.Map(lawyerUpdateProfilePictureRequest, lawyer);

            _unitOfWork.GetRepository<string, Lawyer>().Update(lawyer);
            int result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
                throw new ServerErrorExceptionText("Failed to update profile picture. Please try again later.");
        }
    }
}
