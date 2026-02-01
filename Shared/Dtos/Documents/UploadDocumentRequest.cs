using Domain.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Documents
{
    public class UploadDocumentRequest
    {
        public IFormFile File { get; set; }
        public string? FilePath { get; set; }
        public DocumentType Type { get; set; }
    }
}
