using Shared.Dtos.Cases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Lawyers
{
    public class DashboardResponse
    {
        public int TotalActiveCases { get; set; } // عدد القضايا النشطة
        public int TotalClosedCases { get; set; } // عدد القضايا المغلقة
        public int TotalOnHoldCases { get; set; } // عدد القضايا المعلقة
        public int UpcomingSessionsCountPerWeek { get; set; } // عدد الجلسات القادمة في الأسبوع
        public int DecisionsWithAppealDeadlineThisWeek { get; set; } // عدد الأحكام التي لها موعد نهائي للاستئناف هذا الأسبوع
        public int TodaysSessionsCount { get; set; } // عدد الجلسات اليوم
        public int LastCasesAddedCountPerMonth { get; set; } // عدد القضايا الجديدة التي تم إضافتها في الشهر
        public int UnderReviewedAppealsCount { get; set; } // عدد الاستئنافات المعلقة
 
    }
}
