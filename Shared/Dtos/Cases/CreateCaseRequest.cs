using Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Cases
{
    public class CreateCaseRequest
    {
        public string Title { get; set; } // عنوان القضية  
        public string Description { get; set; } // وصف القضية
        public string FileNumber { get; set; } // رقم الملف في المحكمة
        public string CourtName { get; set; } // اسم المحكمة
        public string? Notes { get; set; } // ملاحظات إضافية
        public Jurisdiction Jurisdiction { get; set; } // الاختصاص القضائي
        public CrimeCategory CrimeCategory { get; set; } // فئة الجريمة
        public CrimeType crimeType { get; set; } // نوع الجريمة
        public string ClientName { get; set; } // اسم العميل
    }
}
