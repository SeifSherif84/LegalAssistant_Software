using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Lawyers
{
    public class LawyerUpdateProfilePictureRequest
    {
        public IFormFile? ProfilePicture { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
