using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Exceptions.NotFound;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.Documents
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet("GetDocumentTypes")] // GET: api/documents/GetDocumentTypes
        [Authorize]
        public IActionResult GetDocumentTypes()
        {
            var documentTypes = Enum.GetValues(typeof(DocumentType))
                                    .Cast<DocumentType>()
                                     .Select(dt => new
                                     {
                                         Id = (int)dt,
                                         Name = dt.ToString()
                                     });
            return Ok(documentTypes);
        }



        [HttpPost("UploadDocument/case/{caseId}")] // POST: api/documents/UploadDocument/case/{caseId}
        public async Task<IActionResult> UploadDocument([FromRoute] int caseId, [FromForm] UploadDocumentRequest uploadDocumentRequest)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get Value Directly From Identifier Claim
            await _serviceManager.DocumentService.UploadDocumentAsync(caseId, lawyerId, uploadDocumentRequest);
            return Ok("Document uploaded successfully.");
        }



        [HttpGet("GetAllDocuments/case/{caseId}")] // GET: api/documents/GetAllDocuments/case/{caseId}
        [Authorize]
        public async Task<IActionResult> GetAllDocuments([FromRoute] int caseId)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var documents = await _serviceManager.DocumentService.GetAllDocumentsAsync(caseId, lawyerId);
            return Ok(documents);
        }



        [HttpDelete("DeleteDocument/{documentId}")] 
        [Authorize]
        public async Task<IActionResult> DeleteDocument([FromRoute] int documentId)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.DocumentService.DeleteDocumentAsync(documentId, lawyerId);
            return Ok("Document deleted successfully.");
        }

    }
}
