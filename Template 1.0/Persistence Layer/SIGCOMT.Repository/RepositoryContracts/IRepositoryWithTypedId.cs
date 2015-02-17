using System;
using System.Linq;
using System.Linq.Expressions;
using SIGCOMT.Common;
using SIGCOMT.Repository.Specifications;

namespace SIGCOMT.Repository.RepositoryContracts
{
    public interface IRepositoryWithTypedId<T, in TId> where T : class
    {
        Expression<Func<T, bool>> Filter { get; set; }

        void Add(T entity);

        T AddGet(T entity);

        void Update(T entity);

        void Delete(T entity);

        IQueryable<T> FindAll();

        IQueryable<T> FindAll(IQuerySpecification<T> specification);

        IQueryable<T> FindAll(Expression<Func<T, bool>> expression);

        T FindOne(IQuerySpecification<T> specification);

        T FindOne(Expression<Func<T, bool>> expression);

        T FindOne(TId id);

        int Count(Expression<Func<T, bool>> expression);

        IQueryable<T> FindAllPaging(FilterParameters<T> parameters);
    }
}