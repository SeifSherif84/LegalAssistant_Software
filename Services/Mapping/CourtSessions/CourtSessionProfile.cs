using AutoMapper;
using Domain.Entities;
using Shared.Dtos.CourtSessions;
using Shared.Dtos.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping.CourtSessions
{
    public class CourtSessionProfile : Profile
    {
        public CourtSessionProfile()
        {
            CreateMap<CreateCourtSession, CourtSession>();
            CreateMap<CourtSession, CourtSessionResponse>();
            CreateMap<UpdateCourtSession, CourtSession>();
            //self mapping to create a new session based on an existing one, while ignoring certain properties that should not be copied
            CreateMap<CourtSession, CourtSession>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.NextSessionId, opt => opt.Ignore()) 
                .ForMember(dest => dest.SessionDate, opt => opt.Ignore())
                .ForMember(dest => dest.SessionStatus, opt => opt.Ignore()) 
                .ForMember(dest => dest.Decisions, opt => opt.Ignore());

            CreateMap<CourtSession, CourtSessionResponseForDashboard>()
                .ForMember(Destination => Destination.caseTitle,
                           Config => Config.MapFrom(Source => Source.Case.Title))
                .ForMember(Destination => Destination.caseFileNumber,
                           Config => Config.MapFrom(Source => Source.Case.FileNumber))
                .ForMember(Destination => Destination.caseStatus,
                           Config => Config.MapFrom(Source => Source.Case.Status));
        }
    }
}
