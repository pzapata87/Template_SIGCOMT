using System;
using System.Collections.Generic;
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
    public class PermisoRolBL : IPermisoRolBL
    {
        private readonly IPermisoRolRepository _permisoUsuarioRepository;

        public PermisoRolBL(IPermisoRolRepository permisoUsuarioRepository)
        {
            _permisoUsuarioRepository = permisoUsuarioRepository;
        }

        public PermisoRol GetById(long id)
        {
            return _permisoUsuarioRepository.FindOne(p => p.Id == id);
        }

        public PermisoRol Get(Expression<Func<PermisoRol, bool>> where)
        {
            return _permisoUsuarioRepository.FindOne(where);
        }

        public IEnumerable<PermisoRol> GetAll()
        {
            return _permisoUsuarioRepository.FindAll();
        }

        public IEnumerable<PermisoRol> GetAll(Expression<Func<PermisoRol, bool>> where)
        {
            return _permisoUsuarioRepository.FindAll(where);
        }

        public void Delete(PermisoRol entity)
        {
            _permisoUsuarioRepository.Delete(entity);
        }

        public int Count(Expression<Func<PermisoRol, bool>> where)
        {
            return _permisoUsuarioRepository.Count(where);
        }

        public IQueryable<PermisoRol> GetAll(FilterParameters<PermisoRol> parameters)
        {
            return _permisoUsuarioRepository.FindAllPaging(parameters);
        }
    }
}