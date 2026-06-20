using Domain.Entities.ChatBotAIEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IBaseSpecifications<TKey, TEntity> where TEntity : class
    {
        List<Expression<Func<TEntity, object>>> Includes { get; set; }
        Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; set; }
        public bool IsPaginationEnabled { get; set; }
        public int Take { get; set; }
    }
}
