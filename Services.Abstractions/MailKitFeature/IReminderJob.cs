using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions.MailKitFeature
{
    public interface IReminderJob
    {
        Task SendPendingRemindersAsync();
    }
}
