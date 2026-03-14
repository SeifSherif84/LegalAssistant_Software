using Services.Abstractions.Authentications;
using Services.Abstractions.Cases;
using Services.Abstractions.ChatBot;
using Services.Abstractions.CourtSessions;
using Services.Abstractions.Documents;
using Services.Abstractions.Lawyers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IServiceManager
    {
        IAuthenticationService AuthenticationService { get; }
        ILawyerService LawyerService { get; }
        ICaseService CaseService { get; }
        IDocumentService DocumentService { get; }
        ICourtSessionService CourtSessionService { get; }
        IChatBotService ChatBotService { get; }
    }
}
