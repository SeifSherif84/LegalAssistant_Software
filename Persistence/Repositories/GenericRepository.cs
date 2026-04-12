using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;
using Shared.Dtos.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class GenericRepository<TKey, TEntity> (AppDbContext _appDbContext) : IGenericRepository<TKey, TEntity> where TEntity : class
    {

        public async Task Add(TEntity entity)
        {
            await _appDbContext.Set<TEntity>().AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _appDbContext.Set<TEntity>().Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _appDbContext.Set<TEntity>().Remove(entity);
        }

        public async Task<TEntity?> GetByIdAsync(IBaseSpecifications<TKey, TEntity> specifications)
        {
            return await ApplySpecifications(specifications).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(IBaseSpecifications<TKey, TEntity> specifications)
        {
            return await ApplySpecifications(specifications).ToListAsync();
        }

        private IQueryable<TEntity> ApplySpecifications(IBaseSpecifications<TKey, TEntity> specifications)
        {
            return SpecificationsEvaluator.GenerateQuery(_appDbContext.Set<TEntity>(), specifications);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _appDbContext.Set<TEntity>().CountAsync(expression);
        }

        public async Task<bool> AnyAsync(IBaseSpecifications<TKey, TEntity> spec)
        {
            return await ApplySpecifications(spec).AnyAsync();
        }


    }
}
