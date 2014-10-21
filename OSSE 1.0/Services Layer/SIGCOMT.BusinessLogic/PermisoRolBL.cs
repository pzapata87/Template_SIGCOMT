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

        public PermisoFormularioRol GetById(long id)
        {
            return _permisoUsuarioRepository.FindOne(p => p.Id == id);
        }

        public PermisoFormularioRol Get(Expression<Func<PermisoFormularioRol, bool>> where)
        {
            return _permisoUsuarioRepository.FindOne(where);
        }

        public IEnumerable<PermisoFormularioRol> GetAll()
        {
            return _permisoUsuarioRepository.FindAll();
        }

        public IEnumerable<PermisoFormularioRol> GetAll(Expression<Func<PermisoFormularioRol, bool>> where)
        {
            return _permisoUsuarioRepository.FindAll(where);
        }

        public void Delete(PermisoFormularioRol entity)
        {
            _permisoUsuarioRepository.Delete(entity);
        }

        public int Count(Expression<Func<PermisoFormularioRol, bool>> where)
        {
            return _permisoUsuarioRepository.Count(where);
        }

        public IQueryable<PermisoFormularioRol> GetAll(FilterParameters<PermisoFormularioRol> parameters)
        {
            return _permisoUsuarioRepository.FindAllPaging(parameters);
        }
    }
}