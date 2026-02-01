using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<TKey, TEntity> (AppDbContext _appDbContext) : IGenericRepository<TKey, TEntity> where TEntity : class
    {
        public async Task<TEntity?> GetByIdAsync(IBaseSpecifications<TKey, TEntity> specifications)
        {
            return await ApplySpecifications(specifications).FirstOrDefaultAsync();
        }

        public async Task Add(TEntity entity)
        {
            await _appDbContext.Set<TEntity>().AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(IBaseSpecifications<TKey, TEntity> specifications)
        {
            return await ApplySpecifications(specifications).ToListAsync();
        }

        private IQueryable<TEntity> ApplySpecifications(IBaseSpecifications<TKey, TEntity> specifications)
        {
            return SpecificationsEvaluator.GenerateQuery(_appDbContext.Set<TEntity>(), specifications);
        }

    }
}
