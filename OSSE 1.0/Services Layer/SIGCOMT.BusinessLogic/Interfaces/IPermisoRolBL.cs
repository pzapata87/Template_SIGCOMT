using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SIGCOMT.BusinessLogic.Core;
using SIGCOMT.Domain;

namespace SIGCOMT.BusinessLogic.Interfaces
{
    public interface IPermisoRolBL : IPaging<PermisoRol>
    {
        PermisoRol GetById(long id);
        PermisoRol Get(Expression<Func<PermisoRol, bool>> where);
        IEnumerable<PermisoRol> GetAll();
        IEnumerable<PermisoRol> GetAll(Expression<Func<PermisoRol, bool>> where);
        void Delete(PermisoRol entity);
    }
}