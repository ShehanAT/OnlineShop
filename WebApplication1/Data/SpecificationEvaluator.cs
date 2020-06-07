using Microsoft.EntityFrameworkCore;
using Microsoft.WebApplication1.Interfaces;
using Microsoft.WebApplication1.Entities;
using System.Linq;


namespace Microsoft.WebApplication1.Data
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;
            if(spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            query = spec.Includes.Aggregate(query,
                (current, include) => current.Include(include)); // Include uses IIncludeQuery

            query = spec.IncludeStrings.Aggregate(query,
                (current, include) => current.Include(include));

            if(spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if(spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            if(spec.GroupBy != null)
            {
                // SelectMany combines many sequences to one
                query = query.GroupBy(spec.GroupBy).SelectMany(x => x);
            }

            if(spec.isPagingEnabled) // enable paging if requested
            {
                query = query.Skip(spec.Skip)
                            .Take(spec.Take);
            }

            return query;
        }
    }
}
