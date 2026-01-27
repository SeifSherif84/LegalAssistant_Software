using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Contexts;
using Persistence.Repositories;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G02.Persistence
{
    public class UnitOfWork(AppDbContext _appDbContext) : IUnitOfWork
    {
        private ConcurrentDictionary<string, object> _repositories = new ConcurrentDictionary<string, object>();

        public IGenericRepository<TKey, TEntity> GetRepository<TKey, TEntity>() where TEntity : class
        {
            return (IGenericRepository<TKey, TEntity>)_repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TKey, TEntity>(_appDbContext));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync();
        }
    }
}
