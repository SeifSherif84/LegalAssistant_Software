using AutoMapper;
using Domain.Entities;
using Domain.Events.Decisions;
using Shared.Dtos.CourtSessions;
using Shared.Dtos.Decisions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping.Decisions
{
    public class DecisionProfile : Profile
    {
        public DecisionProfile()
        {
            CreateMap<DecisionRequest, Decision>();
            CreateMap<Decision,DecisionResponse>()
                .ForMember(D=>D.DecisionType,S=>S.MapFrom(O=>O.DecisionType.ToString()))
                .ForMember(D=> D.SentenceType,S=>S.MapFrom(O=>O.SentenceType.ToString()))
                .ForMember(D => D.AppealsCount,S => S.MapFrom(O => O.Appeals != null ? O.Appeals.Count : 0))
                .ForMember(D=>D.CourtSessionDate,S => S.MapFrom(O=>O.CourtSession.SessionDate))
                .ForMember(d => d.IsAppealWindowOpen,S => S.MapFrom(O =>O.Appealable && DateTime.UtcNow <= O.AppealDeadline));
            

        }
    }
}
