using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SIGCOMT.BusinessLogic.Core;
using SIGCOMT.Domain;

namespace SIGCOMT.BusinessLogic.Interfaces
{
    public interface IPermisoFormularioRolBL : IPaging<PermisoFormularioRol>
    {
        PermisoFormularioRol GetById(long id);
        PermisoFormularioRol Get(Expression<Func<PermisoFormularioRol, bool>> where);
        IEnumerable<PermisoFormularioRol> GetAll();
        IEnumerable<PermisoFormularioRol> GetAll(Expression<Func<PermisoFormularioRol, bool>> where);
        void Delete(PermisoFormularioRol entity);
    }
}