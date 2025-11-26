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
                  .WithOne()
                  .HasForeignKey<CaseParty>(CP => CP.PersonId)
                  .OnDelete(DeleteBehavior.Restrict);

           builder.HasOne(CP => CP.Lawyer)
                  .WithOne()
                  .HasForeignKey<CaseParty>(CP => CP.LawyerId)
                  .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
