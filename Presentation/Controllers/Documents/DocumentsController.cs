using Domain.Entities;
using Domain.Entities.Enums;
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
        // api/documents/GetDocumentTypes
        [HttpGet("GetDocumentTypes")]
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


        [HttpPost("{caseId}/UploadDocument")] // POST: api/documents/{caseId}/UploadDocument
        [Authorize]
        public async Task<IActionResult> UploadDocument([FromRoute]int caseId, [FromForm] UploadDocumentRequest uploadDocumentRequest)
        {
            var lawyerId = User.FindFirst(ClaimTypes.NameIdentifier);
            if (lawyerId is null)
                return BadRequest("id claim not found.");
            await _serviceManager.DocumentService.UploadDocumentAsync(caseId, lawyerId.Value, uploadDocumentRequest);
            return Ok("Document uploaded successfully.");
        }


    }
}
