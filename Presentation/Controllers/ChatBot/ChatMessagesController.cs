using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.ChatBot;
using Shared.Dtos.ChatBot;
using Shared.Dtos.ChatBot.Conan;
using Shared.Dtos.ChatBot.Conan.Responses;
using Shared.Dtos.ChatBot.New;
using System.Security.Claims;

namespace Presentation.Controllers;

[ApiController]
[Route("api/chat-sessions/{sessionId:int}")]
[Authorize]
public class ChatMessagesController(IChatBotService _chatBotService) : ControllerBase
{
    private string LawyerId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;


    // ── Messages ──────────────────────────────────────────────────────────────

    // GET /api/chat-sessions/{sessionId}/messages
    [HttpGet("messages")]
    [ProducesResponseType(typeof(IEnumerable<ChatMessagesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMessages([FromRoute] int sessionId)
    {
        var messages = await _chatBotService.GetMessagesAsync(sessionId, LawyerId);
        return Ok(messages);
    }


    // POST /api/chat-sessions/{sessionId}/messages
    [HttpPost("messages")]
    [ProducesResponseType(typeof(SendMessageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SendMessage([FromRoute] int sessionId, [FromBody] SendMessageRequest request)
    {
        var response = await _chatBotService.SendMessageAsync(LawyerId, sessionId, request);
        return Ok(response);
    }


    // ── Document attachments ──────────────────────────────────────────────────

    // POST /api/chat-sessions/{sessionId}/attachments
    [HttpPost("attachments")]
    [ProducesResponseType(typeof(ConanAttachResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AttachDocument(
        [FromRoute] int sessionId,
        IFormFile file)
    {
        if (file is null || file.Length == 0)
            return BadRequest("يرجى إرفاق ملف صالح.");

        var result = await _chatBotService.AttachDocumentAsync(LawyerId, sessionId, file);
        if (result is null)
            return BadRequest("فشل إرفاق الملف. تأكد من إرسال رسالة واحدة على الأقل قبل الإرفاق.");

        return Ok(result);
    }


    // GET /api/chat-sessions/{sessionId}/attachments
    [HttpGet("attachments")]
    [ProducesResponseType(typeof(ConanAttachmentsListResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAttachments([FromRoute] int sessionId)
    {
        var result = await _chatBotService.GetAttachmentsAsync(LawyerId, sessionId);
        if (result is null) return Ok(new { attachments = Array.Empty<object>() });
        return Ok(result);
    }


    // DELETE /api/chat-sessions/{sessionId}/attachments/{docId}
    [HttpDelete("attachments/{docId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveAttachment( [FromRoute] int sessionId, [FromRoute] string docId)
    {
        var removed = await _chatBotService.RemoveAttachmentAsync(LawyerId, sessionId, docId);
        return removed ? NoContent() : NotFound();
    }


    // DELETE /api/chat-sessions/{sessionId}/attachments
    [HttpDelete("attachments")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ClearAttachments([FromRoute] int sessionId)
    {
        var cleared = await _chatBotService.ClearAttachmentsAsync(LawyerId, sessionId);
        return cleared ? NoContent() : NotFound();
    }
}