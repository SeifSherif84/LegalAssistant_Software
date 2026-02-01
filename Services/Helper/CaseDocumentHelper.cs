using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helper
{
    public static class CaseDocumentHelper
    {
        public static string UploadDocument(IFormFile file, string folderName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files\Cases", folderName);
            string fileName = $"{Guid.NewGuid()}{file.FileName}";
            string filePath = Path.Combine(folderPath, fileName);
            using FileStream fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);
            return fileName;
        }

    }
}
