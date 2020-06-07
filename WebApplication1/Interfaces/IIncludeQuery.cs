using Microsoft.WebApplication1.Helpers.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Interfaces
{   // two interfaces inside one namespace is possible
    public interface IIncludeQuery
    {
        Dictionary<IIncludeQuery, string> PathMap { get; }
        IncludeVisitor Visitor { get; }
        HashSet<string> Paths { get; }
    }
        public interface IIncludeQuery<TEntity, out TPreviousProperty> : IIncludeQuery
        {
        /*
             this interface implements above interface
         * 
         this interface is a reference type for EntityFrameworkQueryableExtensions.Include<TEntity>(IQueryable<TEntity>, string)
         */
        }

}
