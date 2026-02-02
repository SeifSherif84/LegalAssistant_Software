using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Exceptions.BadRequest;
using Domain.Exceptions.NotFound;
using Services.Abstractions.Documents;
using Services.Helper;
using Services.Specifications.Cases;
using Shared.Dtos.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Documents
{
    public class DocumentService(IMapper _mapper,
                                 IUnitOfWork _unitOfWork) : IDocumentService
    {
        public async Task UploadDocumentAsync(int caseId, string lawyerId, UploadDocumentRequest uploadDocumentRequest)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            // 2. Check Case Existence
            var caseSpecifications = new CaseSpecifications(caseId);
            var caseEntity = await _unitOfWork.GetRepository<int, Case>().GetByIdAsync(caseSpecifications);
            if (caseEntity is null)
                throw new CaseNotFoundException($"Case with id : {caseId} not found.");

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
