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
    public class LowerConfigurations : IEntityTypeConfiguration<Lawyer>
    {
        public void Configure(EntityTypeBuilder<Lawyer> builder)
        {
            builder.Property(L => L.HourlyRate).HasColumnType("decimal(18,2)");
        }
    }
}
