using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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


            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.BaseType == null && typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                    builder.Entity(entityType.ClrType).HasQueryFilter(GetIsDeletedRestriction(entityType.ClrType));
            }


            static LambdaExpression GetIsDeletedRestriction(Type type)
            {
                ParameterExpression parameter = Expression.Parameter(type, "entity");
                MemberExpression property = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
                ConstantExpression falseConstant = Expression.Constant(false);
                BinaryExpression condition = Expression.Equal(property, falseConstant);
                LambdaExpression lamdaCondition = Expression.Lambda(condition, parameter);
                return lamdaCondition;
            }

        }



        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                          .Where(E => E.State is EntityState.Modified && E.Entity is ISoftDelete)
                          .ToList();

            foreach (var entry in entries)
            {
                var isDeletedProperty = entry.Property(nameof(ISoftDelete.IsDeleted));
                if (isDeletedProperty.IsModified && (bool)isDeletedProperty.CurrentValue == true)
                {
                    await CascadeSoftDelete(entry);
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }


        private async Task CascadeSoftDelete(EntityEntry entry)
        {
            foreach(var navigationalProperty in entry.Navigations)
            {
                if(!navigationalProperty.IsLoaded)
                    await navigationalProperty.LoadAsync();

                if(navigationalProperty is CollectionEntry collectionEntry &&
                   navigationalProperty.CurrentValue is not null)
                {
                    foreach(var dependentEntity in collectionEntry.CurrentValue)
                    {
                        await ApplySoftDelete(dependentEntity);
                    }
                }
                
                if(navigationalProperty is ReferenceEntry referenceEntry &&
                    navigationalProperty.CurrentValue is not null)
                {
                    await ApplySoftDelete(referenceEntry.CurrentValue);
                }
            }
        }

        private async Task ApplySoftDelete(object entity)
        {
            if (entity is ISoftDelete softDeleteEntity && !softDeleteEntity.IsDeleted && entity is not Lawyer)
            {
                softDeleteEntity.IsDeleted = true;
                softDeleteEntity.DeletedAt = DateTime.UtcNow;
                await CascadeSoftDelete(Entry(entity));
            }
        }
            


    }
}
