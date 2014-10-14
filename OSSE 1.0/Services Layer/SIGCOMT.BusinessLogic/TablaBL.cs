using System;
using System.Linq;
using System.Linq.Expressions;
using SIGCOMT.Aspects;
using SIGCOMT.BusinessLogic.Interfaces;
using SIGCOMT.Common;
using SIGCOMT.Domain;
using SIGCOMT.Repository;

namespace SIGCOMT.BusinessLogic
{
    [TryCatch(ExceptionTypeExpected = typeof (Exception), RethrowException = true)]
    public class TablaBL : ITablaBL
    {
        private readonly ITablaRepository _tablaRepository;

        public TablaBL(ITablaRepository tablaRepository)
        {
            _tablaRepository = tablaRepository;
        }

        public Tabla Get(Expression<Func<Tabla, bool>> where)
        {
            return _tablaRepository.FindOne(where);
        }

        public IQueryable<Tabla> FindAll(Expression<Func<Tabla, bool>> where)
        {
            return _tablaRepository.FindAll(where);
        }

        public int Count(Expression<Func<Tabla, bool>> where)
        {
            return _tablaRepository.Count(where);
        }

        public IQueryable<Tabla> GetAll(FilterParameters<Tabla> parameters)
        {
            return _tablaRepository.FindAll();
        }

        public Tabla GetById(long id)
        {
            return _tablaRepository.FindOne(p => p.Id == id);
        }

        public int Count(Expression<Func<Tabla, bool>> where, Expression<Func<Tabla, object>> group)
        {
            throw new NotImplementedException();
        }
    }
}