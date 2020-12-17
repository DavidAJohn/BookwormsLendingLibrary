using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BookwormsAPI.Entities;

namespace BookwormsAPI.Contracts
{
    public interface IAuthorRepository : IRepositoryBase<Author>
    {
    }
}