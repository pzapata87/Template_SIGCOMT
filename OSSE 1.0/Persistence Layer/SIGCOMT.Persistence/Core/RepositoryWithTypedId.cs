using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using SIGCOMT.Common;
using SIGCOMT.Common.Enum;
using SIGCOMT.Repository.RepositoryContracts;
using SIGCOMT.Repository.Specifications;

namespace SIGCOMT.Persistence.Core
{
    /// <summary>
    ///     Clase Generica para utilizarse como repositorio de Datos
    /// </summary>
    /// <typeparam name="T">Tipo del Domain asociado</typeparam>
    /// <typeparam name="TId">Tipo de dato del parametro utilizado como llave</typeparam>
    public class RepositoryWithTypedId<T, TId> : IRepositoryWithTypedId<T, TId> where T : class
    {
        private readonly DbContext _instanceDB;
        public IDbSet<T> Set;

        public RepositoryWithTypedId(DbContext instanceDbContext)
        {
            _instanceDB = instanceDbContext;
            Set = _instanceDB.Set<T>();
        }

        public Expression<Func<T, bool>> Filter { get; set; }

        public void Add(T entity)
        {
            Set.Add(entity);
        }

        public T AddGet(T entity)
        {
            return Set.Add(entity);
        }

        public void Update(T entity)
        {
        }

        public void Delete(T entity)
        {
            Set.Remove(entity);
        }

        public IQueryable<T> FindAll()
        {
            return Set;
        }

        public IQueryable<T> FindAll(IQuerySpecification<T> specification)
        {
            return specification.SatisfyingElementsFrom(Set);
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> expression)
        {
            return FindAll(new AdHocSpecification<T>(expression));
        }

        public T FindOne(IQuerySpecification<T> specification)
        {
            return specification.SatisfyingElementsFrom(Set).SingleOrDefault();
        }

        public T FindOne(Expression<Func<T, bool>> expression)
        {
            return FindOne(new AdHocSpecification<T>(expression));
        }

        public T FindOne(TId id)
        {
            return Set.Find(new object[] {id});
        }

        public int Count(Expression<Func<T, bool>> expression)
        {
            return Set.AsExpandable().Where(expression).Count();
        }

        public IQueryable<T> FindAllPaging(FilterParameters<T> parameters)
        {
            IQueryable<T> listaAPaginar = GetListContext().Where(parameters.WhereFilter);
            dynamic orderBy = Helper.LambdaPropertyOrderBy<T>(parameters.ColumnOrder);

            listaAPaginar = (parameters.OrderType == TipoOrden.Asc)
                ? Queryable.OrderBy(listaAPaginar, orderBy)
                : Queryable.OrderByDescending(listaAPaginar, orderBy);

            return listaAPaginar.Skip(parameters.Start).Take(parameters.AmountRows);
        }

        private IQueryable<T> GetListContext()
        {
            return Filter == null ? Set : Set.Where(Filter);
        }

        public virtual object[] GetKey(T entity)
        {
            throw new NotImplementedException();
        }
    }
}