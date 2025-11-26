using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class NotificationConfigurations : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasOne(N => N.User)
                   .WithMany()
                   .HasForeignKey(N => N.UserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
