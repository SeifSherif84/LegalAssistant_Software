using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class AppealConfigurations : IEntityTypeConfiguration<Appeal>
    {
        public void Configure(EntityTypeBuilder<Appeal> builder)
        {
            // 1. العلاقة مع القرار الأصلي (OriginalDecision)
            builder.HasOne(a => a.OriginalDecision)
                   .WithMany(d => d.Appeals) 
                   .HasForeignKey(a => a.DecisionId)
                   .OnDelete(DeleteBehavior.Restrict); // منع الحذف التلقائي المتسلسل

            // 2. العلاقة مع القرار الناتج عن الاستئناف (ResultDecision)
            builder.HasOne(a => a.ResultDecision)
                   .WithMany()
                   .HasForeignKey(a => a.ResultDecisionId)
                   .OnDelete(DeleteBehavior.Restrict); // ضروري جداً لتجنب Multiple Cascade Paths

            // 3. العلاقة مع القضية (Case)
            builder.HasOne(a => a.Case)
                   .WithMany(C => C.Appeals)
                   .HasForeignKey(a => a.CaseId)
                   .OnDelete(DeleteBehavior.NoAction); // لو القضية اتمسحت، الاستئناف يفضل موجود

            // 4. العلاقة مع المحامي (Lawyer)
            builder.HasOne(a => a.Lawyer)
                   .WithMany(L => L.Appeals)
                   .HasForeignKey(a => a.LawyerId)
                   .OnDelete(DeleteBehavior.SetNull); // لو المحامي اتمسح، الاستئناف يفضل موجود
        }
    }
}
