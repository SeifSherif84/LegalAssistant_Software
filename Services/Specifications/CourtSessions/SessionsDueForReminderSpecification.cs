using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications.CourtSessions
{
    public class SessionsDueForReminderSpecification : BaseSpecifications<int, CourtSession>
    {
        public SessionsDueForReminderSpecification(DateTime now)
        {
            Criteria = S => S.ReminderDate <= now && !S.ReminderSent;
            Includes.Add(S => S.Case.Lawyers); // Include the related Case entity
        }
    }
}
