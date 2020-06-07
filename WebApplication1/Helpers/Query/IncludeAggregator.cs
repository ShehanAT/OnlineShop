using Microsoft.WebApplication1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Helpers.Query
{
    public class IncludeAggregator<TEntity>
    {
        public IIncludeQuery<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> selector)
        {
            var visitor = new IncludeVisitor();
            visitor.Visit(selector);

            var pathMap = new Dictionary<IIncludeQuery, string>();
            var query = new IncludeQuery<TEntity, TProperty>(pathMap);

            if(!string.IsNullOrEmpty(visitor.Path))
            {
                pathMap[query] = visitor.Path;
            }

            return query;
        }
        

    }
}
