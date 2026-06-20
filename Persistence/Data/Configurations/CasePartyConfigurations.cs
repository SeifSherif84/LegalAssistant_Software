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
    public class CasePartyConfigurations : IEntityTypeConfiguration<CaseParty>
    {
        public void Configure(EntityTypeBuilder<CaseParty> builder)
        {
            builder.HasOne(CP => CP.Person)
                   .WithMany() // تأكد إنها Many مش One
                   .HasForeignKey(CP => CP.PersonId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(CP => CP.Lawyer)
                   .WithMany() // تم التغيير من WithOne لـ WithMany
                   .HasForeignKey(CP => CP.LawyerId) // شلنا النوع الـ Generic
                   .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(cp => cp.Case)
                   .WithMany(c => c.CaseParties)
                   .HasForeignKey(cp => cp.CaseId)
                   .OnDelete(DeleteBehavior.Cascade); // لو القضية اتحذفت، أطرافها يتحذفوا
        }
    }
}
