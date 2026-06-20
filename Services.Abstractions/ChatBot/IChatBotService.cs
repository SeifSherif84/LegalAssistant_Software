using Shared.Dtos.ChatBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.ChatBot
{
    public interface IChatBotService
    {
        Task<int> StartNewChatAsync(string lawyerId, CreateNewChatRequest createNewChatRequest);
        Task<string> SendMessageAsync(string lawyerId,
                                      int chatSessionId,
                                      SendMessageRequest sendMessageRequest);
        Task<IEnumerable<ChatSessionResponse>> MyChatSessionsAsync(string lawyerId, string? search);
        Task<IEnumerable<ChatMessagesResponse>> GetMessagesAsync(int chatSessionId, string lawyerId);
        Task DeleteChatAsync(int chatSessionId, string lawyerId);
        Task UpdateChatTitleAsync(int chatSessionId, string lawyerId, UpdateChatTitleRequest updateChatTitleRequest);
        Task<ChatSessionResponseToUpdate> GetChatSessionAsync(int chatSessionId, string lawyerId);
    }
}
