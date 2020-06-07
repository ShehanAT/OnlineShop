using Microsoft.EntityFrameworkCore;
using Microsoft.WebApplication1.Entities;
using Microsoft.WebApplication1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Data
{
    ///<typeparam name = "T" ></ typeparam >
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity, IAggregateRoot
    {
        // ef repo = entityFramework Repository, this is the repository pattern 
        // repository pattern = IIS => Controller => Unit of Work(repository & DbContext) => EntityFramework & Database 
        // without repository = IIS => Controller => DbContext => EntityFramework & Database 
        // T is instance of BaseEntity object(CatalogItem, CatalogBrand, CatalogType, etc)
        protected readonly CatalogContext _dbContext;

        public EfRepository(CatalogContext dbContext)
        {
            _dbContext = dbContext;
        }

        // virtual is used to modify a method and allow for it to be overriden in a derived class
        // the implementation of a virtual member can be changed by an overriding member in a derived class
        // it is used in base classes that dervied classes override with the keyword: override
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
        {
            
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            //entity is a generic type object 
            // await is needed to wait for AddAsync() to finish before method execution
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        } 

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(); // await keyword has 'return' functionality 
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            // CountAsync may or may not return a value therefore 'return' is used to return false if CountAsyn() is null
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<T> FirstAsync(ISpecification<T> spec)
        {
            // FirstAsync() returns the first element in source, therefore return type T
            // multiple active operations on the same context instance are not supported, 'await' is needed
            return await ApplySpecification(spec).FirstAsync();
        }

        public async Task<T> FirstOrDefaultAsync(ISpecification<T> spec)
        {
            // returns first element in sequence or default value if no elements in sequence
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }
    }
}
