using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BookwormsAPI.Contracts;
using BookwormsAPI.Entities;
using BookwormsAPI.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BookwormsAPI.Data
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : BaseEntity
    {
        protected ApplicationDbContext _repositoryContext { get; set; }
        public RepositoryBase(ApplicationDbContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public async Task<IEnumerable<T>> ListAllAsync()
        {
            return await _repositoryContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repositoryContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync(); 
        }

        public async Task<IEnumerable<T>> ListAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync(); 
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_repositoryContext.Set<T>().AsQueryable(), spec);
        }

        public async Task<T> Create(T entity)
        {
            await _repositoryContext.Set<T>().AddAsync(entity);
            await Save();
            return entity;
        }

        public async Task<bool> Update(T entity)
        {
            _repositoryContext.Set<T>().Update(entity);
            return await Save();
        }

        public async Task<bool> Delete(T entity)
        {
            _repositoryContext.Set<T>().Remove(entity);
            return await Save();
        }

        public async Task<bool> Save()
        {
            var changes = await _repositoryContext.SaveChangesAsync();
            return changes > 0;
        }
    }
}