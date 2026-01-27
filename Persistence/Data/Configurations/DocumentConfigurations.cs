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
    public class DocumentConfigurations : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasOne(D => D.Lawyer)
                   .WithMany(L => L.Documents)
                   .HasForeignKey(D => D.LawyerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
