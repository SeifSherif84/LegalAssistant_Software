using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;
using Shared.Dtos.ChatBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.ChatBot
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatBotController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost("StartNewChat")] 
        [Authorize]
        public async Task<IActionResult> StartNewChat([FromBody] CreateNewChatRequest createNewChatRequest)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var chatSessionId = await _serviceManager.ChatBotService.StartNewChatAsync(lawyerId, createNewChatRequest);
            return Ok(new {chatSessionId = chatSessionId, Message = "Chat created successfully." });
        }


        [HttpPost("SendMessage/ChatSession/{chatSessionId}")]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromRoute] int chatSessionId, 
                                                     [FromBody] SendMessageRequest sendMessageRequest)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var aiResponseText = await _serviceManager.ChatBotService.SendMessageAsync(lawyerId, chatSessionId, sendMessageRequest);
            return Ok(aiResponseText);
        }


        [HttpGet("MyChatSessions")]
        [Authorize]
        public async Task<IActionResult> MyChatSessions([FromQuery] string? search)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var chatSessionsResponse = await _serviceManager.ChatBotService.MyChatSessionsAsync(lawyerId, search);
            return Ok(chatSessionsResponse);
        }


        [HttpGet("GetMessages/ChatSession/{chatSessionId}")]
        [Authorize]
        public async Task<IActionResult> GetMessages([FromRoute]int chatSessionId)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var MessagesResponse = await _serviceManager.ChatBotService.GetMessagesAsync(chatSessionId, lawyerId);
            return Ok(MessagesResponse);
        }


        [HttpDelete("DeleteChatSession/{chatSessionId}")]
        [Authorize]
        public async Task<IActionResult> DeleteChatSession([FromRoute] int chatSessionId)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.ChatBotService.DeleteChatAsync(chatSessionId, lawyerId);
            return Ok("Chat deleted successfully.");
        }



        [HttpPut("UpdateChatTitle/{chatSessionId}")]
        [Authorize]
        public async Task<IActionResult> UpdateChatTitle([FromRoute] int chatSessionId,
                                                         [FromBody] UpdateChatTitleRequest updateChatTitleRequest)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _serviceManager.ChatBotService.UpdateChatTitleAsync(chatSessionId, lawyerId, updateChatTitleRequest);
            return Ok("Chat Title updated successfully.");
        }



        [HttpGet("GetChatSession/{chatSessionId}")]
        [Authorize]
        public async Task<IActionResult> GetChatSession([FromRoute] int chatSessionId)
        {
            var lawyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var chatSessionResponseToUpdate = await _serviceManager.ChatBotService.GetChatSessionAsync(chatSessionId, lawyerId);
            return Ok(chatSessionResponseToUpdate);
        }



    }
}
