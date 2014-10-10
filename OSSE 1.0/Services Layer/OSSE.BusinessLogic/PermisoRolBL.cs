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
    [TryCatch(ExceptionTypeExpected = typeof(Exception), RethrowException = true)]
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
