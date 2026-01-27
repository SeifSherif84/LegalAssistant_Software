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
    public class DecisionConfigurations : IEntityTypeConfiguration<Decision>
    {
        public void Configure(EntityTypeBuilder<Decision> builder)
        {
            builder.HasOne(d => d.CourtSession)
                   .WithMany(cs => cs.Decisions)
                   .HasForeignKey(d => d.CourtSessionId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
