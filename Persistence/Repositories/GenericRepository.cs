using Domain.Contracts;
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
        public async Task<TEntity?> GetByIdAsync(TKey key)
        {
            var entity = await _appDbContext.Set<TEntity>().FindAsync(key);
            return entity;
        }
    }
}
