using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    public enum DocumentType
    {
        PoliceReport = 1, // تقرير شرطة
        PowerOfAttorney = 2, // توكيع
        IdentityDocument = 3, // بطاقة هوية
        MedicalReport = 4, // تقرير طبي
        EvidencePhoto = 5, // صورة دليل
        PleadingMemo = 6, // مذكرة دفوع
        CourtOrder = 7, // أمر محكمة
        Translation = 8, // ترجمة
        FullCaseFile = 9, // (ملف القضية الكامل (التحقيقات + المحضر + التحريات)
        InvestigationRecord = 10 // محضر تحقيقات
    }

}
