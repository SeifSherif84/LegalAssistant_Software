using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    public enum DocumentType
    {
        PoliceReport = 1,
        PowerOfAttorney = 2,
        IdentityDocument = 3,
        MedicalReport = 4,
        EvidencePhoto = 5,
        PleadingMemo = 6,
        CourtOrder = 7,
        Translation = 8,
        FullCaseFile = 9,      // ملف القضية الكامل (التحقيقات + المحضر + التحريات)
        InvestigationRecord = 10
    }

}
