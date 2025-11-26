using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Log : BaseEntity<int>
    {
        public Guid UserId { get; set; } // Foreign key to UserApp
        public UserApp User { get; set; } // 


        // نوع الأكشن اللي حصل (Login, CreateCase, UpdateCase، رفع مستند...)
        public string Action { get; set; }


        // شرح تفصيلي للّي حصل
        public string Description { get; set; }


        // وقت حدوث اللوج
        public DateTime Timestamp { get; set; }


        // لو الحدث مرتبط بقضية معينة
        public int? CaseId { get; set; }
        public Case? Case { get; set; }


        // لو الحدث مرتبط بجلسة معينة
        public int? CourtSessionId { get; set; }
        public CourtSession? CourtSession { get; set; }


        // الـ IP اللي حصل منه الاكشن (مهم للأمان والتحقيق في أي اختراق)
        public string IPAddress { get; set; }


        // القيم قبل وبعد التعديل (مهم للمراجعة والتدقيق)
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
    }

}
