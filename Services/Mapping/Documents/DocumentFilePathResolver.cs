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
    public class DocumentFilePathResolver(IConfiguration _configuration) : IValueResolver<Document, DocumentResponse, string>
    {
        public string Resolve(Document source, DocumentResponse destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.FilePath))
            {
                destMember = $"{_configuration["TunnleBaseURL"]}/files/Cases/Documents/{Uri.EscapeDataString(source.FilePath)}";
                return destMember;
            }
            return string.Empty;
        }
    }
}
