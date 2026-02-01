using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Exceptions.BadRequest;
using Services.Abstractions.Documents;
using Services.Helper;
using Shared.Dtos.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Documents
{
    public class DocumentService(IMapper _mapper,
                                 IUnitOfWork _unitOfWork) : IDocumentService
    {
        public async Task UploadDocumentAsync(int caseId, string lawyerId, UploadDocumentRequest uploadDocumentRequest)
        {
            uploadDocumentRequest.FilePath = CaseDocumentHelper.UploadDocument(uploadDocumentRequest.File, "Documents");
            var document = _mapper.Map<Document>(uploadDocumentRequest);
            document.CaseId = caseId;
            document.LawyerId = lawyerId;
            document.Title = uploadDocumentRequest.File.FileName;
            document.UploadedAt = DateTime.UtcNow;
            document.Status = DocumentStatus.Pending;
            document.IsAnalyzedByAI = false;
            document.AnalyzedAt = null;
            await _unitOfWork.GetRepository<int, Document>().Add(document);
            int creationResult = await _unitOfWork.SaveChangesAsync();
            if (creationResult <= 0)
                throw new DocumentUploadedFailed("Failed to upload document.");
        }
    }
}
