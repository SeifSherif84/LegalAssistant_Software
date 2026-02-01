using Domain.Entities.Enums;
using Domain.Entities.HelperClass;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Authentications
{
    public class LawyerRegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; } 
        public string Password { get; set; }

        public IFormFile? ProfilePicture { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public int YearsOfExperience { get; set; }

        // رقم قيد المحامي في نقابة المحامين
        public string BarRegistrationNumber { get; set; }


        // صورة بطاقة القيد
        public IFormFile BarIdCard { get; set; }
        public string? BarIdCardUrl { get; set; }   

    }

}
