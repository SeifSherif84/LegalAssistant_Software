using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Contexts
{
    public class AppDbContext : IdentityDbContext<UserApp>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {

        }

        public DbSet<Lawyer> Lawyers { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<CourtSession> CourtSessions { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Decision> Decisions { get; set; }
        public DbSet<CaseParty> CaseParties { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Appeal> Appeals { get; set; }
        public DbSet<AiAnalysis> AiAnalyses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<UserApp>().ToTable("Users");
            builder.Entity<Lawyer>().ToTable("Lawyers");
            builder.Entity<UserApp>().HasQueryFilter(u => !u.IsDeleted);

            builder.Entity<Document>()
                .HasQueryFilter(d => !d.Lawyer.IsDeleted);

            builder.Entity<Notification>()
                .HasQueryFilter(n => !n.User.IsDeleted);

            builder.Entity<Log>()
                .HasQueryFilter(l => !l.User.IsDeleted);
        }


    }
}
