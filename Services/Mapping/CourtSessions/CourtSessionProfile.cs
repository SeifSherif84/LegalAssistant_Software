using AutoMapper;
using Domain.Entities;
using Shared.Dtos.CourtSessions;
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
            CreateMap<CourtSession, CourtSessionResponseDashboard>()
                .ForMember(Destination => Destination.caseTitle,
                           Config => Config.MapFrom(Source => Source.Case.Title))
                .ForMember(Destination => Destination.caseFileNumber,
                           Config => Config.MapFrom(Source => Source.Case.FileNumber))
                .ForMember(Destination => Destination.caseStatus,
                           Config => Config.MapFrom(Source => Source.Case.Status));
        }
    }
}
