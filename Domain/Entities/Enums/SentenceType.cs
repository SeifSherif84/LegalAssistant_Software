using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    public enum SentenceType
    {
        Acquittal = 1,          // براءة
        Incarceration = 2,      // حبس أو سجن
        LifeImprisonment = 3,   // سجن مؤبد
        DeathPenalty = 4,       // إعدام
        Fine = 5,               // غرامة مالية
        SuspendedSentence = 6,  // مع إيقاف التنفيذ
        JuvenileProbation = 7   // تدابير إصلاحية (للأحداث)
    }
}
