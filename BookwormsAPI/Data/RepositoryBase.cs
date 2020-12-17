using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BookwormsAPI.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BookwormsAPI.Data
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected ApplicationDbContext _repositoryContext { get; set; }
        public RepositoryBase(ApplicationDbContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }
        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await _repositoryContext.Set<T>().AsNoTracking().ToListAsync();
        }
        public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
        {
            return await _repositoryContext.Set<T>().Where(expression).AsNoTracking().ToListAsync();
        }
        public void Create(T entity)
        {
            _repositoryContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            _repositoryContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            _repositoryContext.Set<T>().Remove(entity);
        }
    }

}