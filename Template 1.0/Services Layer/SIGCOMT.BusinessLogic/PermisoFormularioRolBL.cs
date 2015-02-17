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
    public class PermisoFormularioRolBL : IPermisoFormularioRolBL
    {
        private readonly IPermisoFormularioRolRepository _permisoRolRepository;

        public PermisoFormularioRolBL(IPermisoFormularioRolRepository permisoRolRepository)
        {
            _permisoRolRepository = permisoRolRepository;
        }

        public PermisoFormularioRol GetById(long id)
        {
            return _permisoRolRepository.FindOne(p => p.Id == id);
        }

        public PermisoFormularioRol Get(Expression<Func<PermisoFormularioRol, bool>> where)
        {
            return _permisoRolRepository.FindOne(where);
        }

        public IEnumerable<PermisoFormularioRol> GetAll()
        {
            return _permisoRolRepository.FindAll();
        }

        public IEnumerable<PermisoFormularioRol> GetAll(Expression<Func<PermisoFormularioRol, bool>> where)
        {
            return _permisoRolRepository.FindAll(where);
        }

        public void Delete(PermisoFormularioRol entity)
        {
            _permisoRolRepository.Delete(entity);
        }

        public int Count(Expression<Func<PermisoFormularioRol, bool>> where)
        {
            return _permisoRolRepository.Count(where);
        }

        public IQueryable<PermisoFormularioRol> GetAll(FilterParameters<PermisoFormularioRol> parameters)
        {
            return _permisoRolRepository.FindAllPaging(parameters);
        }
    }
}