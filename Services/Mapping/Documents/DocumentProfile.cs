using AutoMapper;
using Domain.Entities;
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
        public DocumentProfile()
        {
            CreateMap<UploadDocumentRequest, Document>();
        }
    }
}
