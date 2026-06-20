using Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class BaseSpecifications<TKey, TEntity> : IBaseSpecifications<TKey, TEntity> where TEntity : class
    {
        public List<Expression<Func<TEntity, object>>> Includes { get; set; }
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; set; }
        public bool IsPaginationEnabled { get; set; }
        public int Take { get; set; }


        public BaseSpecifications()
        {
            Includes = new List<Expression<Func<TEntity, object>>>();
            Criteria = null;
            OrderByDescending = null;
        }
    }
}
