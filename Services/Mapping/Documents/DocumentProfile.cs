using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Shared.Dtos.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Mapping.Documents
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile(IConfiguration _configuration)
        {
            CreateMap<UploadDocumentRequest, Document>();
            CreateMap<Document, DocumentResponse>()
                .ForMember(D => D.FilePath, Config => Config.MapFrom(new DocumentFilePathResolver(_configuration)));
        }
    }
}
