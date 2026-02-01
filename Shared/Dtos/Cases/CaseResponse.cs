using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Cases
{
    public class CaseResponse
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string FileNumber { get; set; }
        public string CourtName { get; set; }
        public CaseStatus Status { get; set; }
        public string? Notes { get; set; }
        public Jurisdiction Jurisdiction { get; set; }
        public CrimeCategory CrimeCategory { get; set; }
        public CrimeType crimeType { get; set; }
        public string ClientName { get; set; }

    }
}
