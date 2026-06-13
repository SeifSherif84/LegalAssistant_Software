using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.ChatBot;
using Shared.Dtos.ChatBot;
using System.Security.Claims;

namespace Presentation.Controllers 
{
    [ApiController]
    [Route("api/chat-sessions")]
    [Authorize]
    public class ChatSessionsController(IChatBotService _chatBotService) : ControllerBase
    {
        private string LawyerId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;


        // POST /api/chat-sessions
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateSession([FromBody] CreateNewChatRequest request)
        {
            var sessionId = await _chatBotService.StartNewChatAsync(LawyerId, request);
            return CreatedAtAction(nameof(GetSession), new { sessionId }, sessionId);
        }


        // GET /api/chat-sessions
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ChatSessionResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMySessions([FromQuery] string? search = null)
        {
            var sessions = await _chatBotService.MyChatSessionsAsync(LawyerId, search);
            return Ok(sessions);
        }


        // GET /api/chat-sessions/{sessionId}
        [HttpGet("{sessionId:int}")]
        [ProducesResponseType(typeof(ChatSessionResponseToUpdate), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetSession([FromRoute] int sessionId)
        {
            var session = await _chatBotService.GetChatSessionAsync(sessionId, LawyerId);
            return Ok(session);
        }


        // PATCH /api/chat-sessions/{sessionId}/title
        [HttpPatch("{sessionId:int}/title")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTitle(
            [FromRoute] int sessionId,
            [FromBody] UpdateChatTitleRequest request)
        {
            await _chatBotService.UpdateChatTitleAsync(sessionId, LawyerId, request);
            return NoContent();
        }


        // DELETE /api/chat-sessions/{sessionId}
        [HttpDelete("{sessionId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSession([FromRoute] int sessionId)
        {
            await _chatBotService.DeleteChatAsync(sessionId, LawyerId);
            return NoContent();
        }
    }
}

