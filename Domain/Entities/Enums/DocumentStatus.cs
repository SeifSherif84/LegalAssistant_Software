using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    public enum DocumentStatus
    {
        Pending = 1,    // الملف اترفع بس لسه متبعتش للـ AI
        Processing = 2, // جاري التحليل حالياً (عشان تظهر Spinner أو Loading للمحامي)
        Completed = 3,  // الـ AI خلص وطلع الثغرات بنجاح
        Failed = 4      // حصلت مشكلة (الملف بايظ أو الـ AI API وقع)
    }

}
