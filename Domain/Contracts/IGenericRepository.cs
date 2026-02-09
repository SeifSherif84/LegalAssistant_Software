using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IGenericRepository<TKey, TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(IBaseSpecifications<TKey, TEntity> specifications);
        Task Add(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllAsync(IBaseSpecifications<TKey, TEntity> specifications);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
