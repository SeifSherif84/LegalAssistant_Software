using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public DbSet<Office> Offices { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<CourtSession> CourtSessions { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Decision> Decisions { get; set; }
        public DbSet<CaseParty> CaseParties { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Appeal> Appeals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<UserApp>().ToTable("Users");
            builder.Entity<Lawyer>().ToTable("Lawyers");
            builder.Entity<Employee>().ToTable("Employees");
        }


    }
}
