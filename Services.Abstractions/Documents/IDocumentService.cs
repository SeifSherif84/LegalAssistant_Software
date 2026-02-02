using Shared.Dtos.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.Documents
{
    public interface IDocumentService
    {
        public Task UploadDocumentAsync(int caseId, string lawyerId, UploadDocumentRequest uploadDocumentRequest);
    }
}
