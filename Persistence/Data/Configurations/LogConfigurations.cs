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
    public class LogConfigurations : IEntityTypeConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
            builder.HasOne(L => L.User)
                   .WithMany()
                   .HasForeignKey(L => L.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
