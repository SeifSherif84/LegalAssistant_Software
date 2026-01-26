using Domain.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class DbInitializer(AppDbContext _appDbContext,
                               RoleManager<IdentityRole> _roleManager) : IDbInitializer
    {
        public async Task InitializerAsync()
        {
            var migrationsPending = await _appDbContext.Database.GetPendingMigrationsAsync();
            if (migrationsPending.Any())
                await _appDbContext.Database.MigrateAsync();

            if (!_appDbContext.Roles.Any())
            {
                IdentityRole Admin = new IdentityRole("Admin");
                IdentityRole Lawyer = new IdentityRole("Lawyer");
                IdentityRole Employee = new IdentityRole("Employee");
                await _roleManager.CreateAsync(Admin);
                await _roleManager.CreateAsync(Lawyer);
                await _roleManager.CreateAsync(Employee);
            }
        }
    }
}
