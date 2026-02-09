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
        }
    }
}
