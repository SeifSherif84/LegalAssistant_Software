using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helper
{
    public static class LawyerImageHelper
    {
        public static string UploadProfilePicture(IFormFile file, string folderName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files\images", folderName); 
            string fileName = $"{Guid.NewGuid()}{file.FileName}"; 
            string filePath = Path.Combine(folderPath, fileName); 
            using FileStream fileStream = new FileStream(filePath, FileMode.Create); 
            file.CopyTo(fileStream);
            return fileName;
        }

        public static void DeleteProfilePicture(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public static string UploadBarIdCardPicture(IFormFile file, string folderName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files\images", folderName);
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            string filePath = Path.Combine(folderPath, fileName);
            using FileStream fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            return fileName;
        }



    }
}
