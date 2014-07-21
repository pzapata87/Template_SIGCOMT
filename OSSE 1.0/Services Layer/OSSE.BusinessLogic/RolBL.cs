using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OSSE.Aspects;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Common;
using OSSE.Domain;
using OSSE.Repository;

namespace OSSE.BusinessLogic
{
    public class RolBL : IRolBL
    {
        private readonly IRolRepository _rolRepository;

        public RolBL(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        [TryCatch(ExceptionTypeExpected = typeof(Exception), RethrowException = true)]
        public int Count(Expression<Func<Rol, bool>> where)
        {
            return _rolRepository.Count(where);
        }

        [TryCatch(ExceptionTypeExpected = typeof(Exception), RethrowException = true)]
        public IQueryable<Rol> GetAll(JQGridParameters<Rol> parameters)
        {
            return _rolRepository.FindAllPaging(parameters);
        }

        [TryCatch(ExceptionTypeExpected = typeof(Exception), RethrowException = true)]
        public Rol GetById(int id)
        {
            return _rolRepository.FindOne(p => p.Id == id);
        }

        [TryCatch(ExceptionTypeExpected = typeof(Exception), RethrowException = true)]
        public Rol Get(Expression<Func<Rol, bool>> where)
        {
            return _rolRepository.FindOne(where);
        }

        [TryCatch(ExceptionTypeExpected = typeof(Exception), RethrowException = true)]
        public IEnumerable<Rol> GetAll(Expression<Func<Rol, bool>> where)
        {
            return _rolRepository.FindAll(where);
        }

        [TryCatch(ExceptionTypeExpected = typeof(Exception), RethrowException = true)]
        public IEnumerable<Rol> GetAll()
        {
            return _rolRepository.FindAll(p => true);
        }
    }
}