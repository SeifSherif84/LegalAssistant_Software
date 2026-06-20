using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Persistence
{
    public static class SpecificationsEvaluator
    {
        public static IQueryable<TEntity> GenerateQuery<TKey, TEntity>(IQueryable<TEntity> baseQuery, 
                                                                       IBaseSpecifications<TKey, TEntity> specifications) 
                                                                       where TEntity : class
        {
            IQueryable<TEntity> generatedQuery = baseQuery; // _context.Set<TEntity>();


            if (specifications.Criteria is not null)
                generatedQuery = generatedQuery.Where(specifications.Criteria); // _context.Set<TEntity>().Where(specifications.Criteria);

            if (specifications.OrderByDescending != null)
                generatedQuery = generatedQuery.OrderByDescending(specifications.OrderByDescending);

            if (specifications.OrderBy != null)
                generatedQuery = generatedQuery.OrderBy(specifications.OrderBy);

            if (specifications.Includes.Count > 0)
                generatedQuery = specifications.Includes.Aggregate(generatedQuery, (currentQuery, includeExpression) => currentQuery.Include(includeExpression)); // _context.Set<TEntity>().Include(includeExpression);

            if (specifications.IsPaginationEnabled)
                generatedQuery = generatedQuery.Take(specifications.Take);

            return generatedQuery;
        }

    }
}
