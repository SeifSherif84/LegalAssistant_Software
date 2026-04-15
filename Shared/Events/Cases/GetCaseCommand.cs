using Domain.Entities;
using MediatR;
using Shared.Dtos.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Events.Cases
{
    public class GetCaseCommand : IRequest<Case>
    {
        public int CaseId { get; set; }
        public GetCaseCommand(int caseId)
        {
            CaseId = caseId;
        }
    }
}
