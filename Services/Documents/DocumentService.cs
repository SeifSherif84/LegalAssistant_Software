using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Exceptions.BadRequest;
using Domain.Exceptions.NotFound;
using Services.Abstractions.Documents;
using Services.Helper;
using Services.Specifications.Cases;
using Services.Specifications.Documents;
using Shared.Dtos.CourtSessions;
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

        public async Task DeleteDocumentAsync(int documentId, string lawyerId)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            // 2. Check Document Existence
            var documentSpecifications = DocumentSpecifications.GetById(documentId, false, false);
            var documentEntity = await _unitOfWork.GetRepository<int, Document>().GetByIdAsync(documentSpecifications);
            if (documentEntity is null)
                throw new DocumentNofFoundException($"Document with id : {documentId} not found.");

            // 3. Check if the document belongs to this lawyer
            if (documentEntity.LawyerId == lawyerId)
            {
                documentEntity.IsDeleted = true;
                documentEntity.DeletedAt = DateTime.UtcNow;
                _unitOfWork.GetRepository<int, Document>().Update(documentEntity);
                int deletionResult = await _unitOfWork.SaveChangesAsync();
                if (deletionResult <= 0)
                    throw new DocumentDeletionFailed("Failed to delete document.");
            }
            else
                throw new UnauthorizedAccessException("You don't have permission to delete this documents.");
        }


        public async Task<IEnumerable<DocumentResponse>> GetAllDocumentsAsync(int caseId, string lawyerId)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            // 2. Check Case Existence
            var caseSpecifications = new CaseSpecifications(caseId, false,
                                                                    false,
                                                                    false,
                                                                    false,
                                                                    false,
                                                                    true,
                                                                    false);
            var caseEntity = await _unitOfWork.GetRepository<int, Case>().GetByIdAsync(caseSpecifications);
            if (caseEntity is null)
                throw new CaseNotFoundException($"Case with id : {caseId} not found.");

            // 2. Check if the case belongs to this lawyer
            if (caseEntity.Lawyers.Any(L => L.Id == lawyerId))
            {
                var documentSpecifications = DocumentSpecifications.GetByCaseId(caseId, false, false);
                var documents = await _unitOfWork.GetRepository<int, Document>().GetAllAsync(documentSpecifications);
                return _mapper.Map<IEnumerable<DocumentResponse>>(documents);
            }
            else
                throw new UnauthorizedAccessException("You don't have permission to get documents for this case.");
        }


        public async Task UploadDocumentAsync(int caseId, string lawyerId, UploadDocumentRequest uploadDocumentRequest)
        {
            // 1. Check LawyerId
            if (string.IsNullOrEmpty(lawyerId))
                throw new LawyerIdentifierMissedException("Lawyer identifier is missing.");

            // 2. Check Case Existence
            var caseSpecifications = new CaseSpecifications(caseId, false,
                                                                    false,
                                                                    false,
                                                                    false,
                                                                    false,
                                                                    true,
                                                                    false);
            var caseEntity = await _unitOfWork.GetRepository<int, Case>().GetByIdAsync(caseSpecifications);
            if (caseEntity is null)
                throw new CaseNotFoundException($"Case with id : {caseId} not found.");

            // 2. Check if the case belongs to this lawyer
            if (caseEntity.Lawyers.Any(L => L.Id == lawyerId))
            {
                uploadDocumentRequest.FilePath = CaseDocumentHelper.UploadDocument(uploadDocumentRequest.File, "Documents");
                var document = _mapper.Map<Document>(uploadDocumentRequest);
                document.CaseId = caseId;
                document.LawyerId = lawyerId;
                document.Title = uploadDocumentRequest.File.FileName;
                document.UploadedAt = DateTime.UtcNow;
                document.Status = DocumentStatus.قيد_الانتظار;
                document.IsAnalyzedByAI = false;
                document.AnalyzedAt = null;
                await _unitOfWork.GetRepository<int, Document>().Add(document);
                int creationResult = await _unitOfWork.SaveChangesAsync();
                if (creationResult <= 0)
                    throw new DocumentUploadedFailed("Failed to upload document.");
            }
            else
                throw new UnauthorizedAccessException("You don't have to upload document for this case.");
        }


    }
}
