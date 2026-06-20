using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Shared.Dtos.Dashboard;
using Shared.Dtos.Lawyers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping.Lawyers
{
    public class LawyerProfile : Profile
    {
        public LawyerProfile(IConfiguration _configuration)
        {
            CreateMap<Lawyer, LawyerResponse>()
                .ForMember(D => D.ProfilePictureUrl, C => C.MapFrom(new LawyerProfilePictureSolver(_configuration)));

            CreateMap<LawyerUpdateRequest, Lawyer>();
            CreateMap<LawyerUpdateProfilePictureRequest, Lawyer>();


            // Need Modify 
            CreateMap<Decision, DecisionResponseForDashboard>()
                .ForMember(D => D.sessionDate, Config => Config.MapFrom(S => S.CourtSession.SessionDate))
                .ForMember(D => D.sessionCourtName, Config => Config.MapFrom(S => S.CourtSession.CourtName))
                .ForMember(D => D.sessionCourtRoom, Config => Config.MapFrom(S => S.CourtSession.CourtRoom))
                .ForMember(D => D.sessionFloor, Config => Config.MapFrom(S => S.CourtSession.Floor))
                .ForMember(D => D.sessionType, Config => Config.MapFrom(S => S.CourtSession.SessionType))
                .ForMember(D => D.sessionStatus, Config => Config.MapFrom(S => S.CourtSession.SessionStatus))

                .ForMember(D => D.caseTitle, Config => Config.MapFrom(S => S.CourtSession.Case.Title))
                .ForMember(D => D.caseFileNumber, Config => Config.MapFrom(S => S.CourtSession.Case.FileNumber))
                .ForMember(D => D.caseStatus, Config => Config.MapFrom(S => S.CourtSession.Case.Status));
        }
    }
}
