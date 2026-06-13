using AutoMapper;
using Domain.Entities.ChatBotAIEntities;
using Shared.Dtos.ChatBot;
using Shared.Dtos.ChatBot.Conan.Responses.ConanSharedModels;
using Shared.Dtos.ChatBot.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping.ChatBot
{
    public class ChatBotProfile : Profile
    {
        public ChatBotProfile()
        {
            CreateMap<CreateNewChatRequest, ChatSession>();
            CreateMap<SendMessageRequest, ChatMessage>();
            CreateMap<ChatSession, ChatSessionResponse>();
            CreateMap<UpdateChatTitleRequest, ChatSession>();
            CreateMap<ChatSession, ChatSessionResponseToUpdate>();
            CreateMap<ChatMessage, ChatMessagesResponse>();
            CreateMap<ConanSourceInfo, SourceInfoResponse>()
                .ForMember(dest => dest.IsArticleLookupRescue,
                           opt => opt.MapFrom(src => src.Source == "[article-lookup rescue]"));
        }
    }
}
