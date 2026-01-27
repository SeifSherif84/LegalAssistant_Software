//using Domain.Entities;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Persistence.Data.Configurations
//{
//    public class OfficeConfigurations : IEntityTypeConfiguration<Office>
//    {
//        public void Configure(EntityTypeBuilder<Office> builder)
//        {
//            builder.OwnsOne(O => O.Address);
//            builder.OwnsOne(O => O.ContactInfo);
//        }
//    }
//}
