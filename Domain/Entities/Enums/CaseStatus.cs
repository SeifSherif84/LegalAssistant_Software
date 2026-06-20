using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Enums
{
    public enum CaseStatus
    {
        Active = 1, // القضية مفتوحة وجارية
        Closed = 2, // القضية مغلقة وتم الانتهاء منها
        OnHold = 3, // القضية معلقة مؤقتاً بسبب غياب أحد الأطراف أو لسبب آخر
        Dismissed = 4, // القضية تم رفضها أو إسقاطها من قبل المحكمة
        Settled = 5 // القضية تم تسويتها خارج المحكمة باتفاق بين الأطراف
    }
}
