using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        //This function has all responsibility for generate  dynamic queries...
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec)
        {
            var query = inputQuery; // _dbContext.Set(TEntity)
           
            if(spec.Criteria is not null)
                query = query.Where(spec.Criteria); // _dbContext.Set(TEntity).Where("Criteria") : "Criteria" >> (P => P.Id == 1).

            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);

            else if(spec.OrderByDesc is not null) 
                query = query.OrderByDescending(spec.OrderByDesc);

            //Includes: may be the query has many includes for example : product has
            //1.Include(P => P.Brand)
            //2.Include(P => P.Category)
            //..So on..
            //So We Use Aggregate Linq Operator to acumlate

            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            
            return query;
        }
    }
}
