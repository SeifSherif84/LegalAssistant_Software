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
    public class CaseConfigurations : IEntityTypeConfiguration<Case>
    {
        public void Configure(EntityTypeBuilder<Case> builder)
        {
            builder.HasMany(C => C.Documents)
                   .WithOne(D => D.Case)
                   .HasForeignKey(D => D.CaseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(C => C.CourtSessions)
                   .WithOne(CS => CS.Case)
                   .HasForeignKey(CS => CS.CaseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(C => C.Decisions)
                   .WithOne(D => D.Case)
                   .HasForeignKey(D => D.CaseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(C => C.CaseParties)
                   .WithOne(CP => CP.Case)
                   .HasForeignKey(CP => CP.CaseId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
