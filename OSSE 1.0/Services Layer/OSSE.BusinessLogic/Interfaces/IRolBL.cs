using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using OSSE.Domain;

namespace OSSE.BusinessLogic.Interfaces
{
    public interface IRolBL
    {
        Rol Get(Expression<Func<Rol, bool>> where);
        Rol GetById(int id);
        IEnumerable<Rol> GetAll(Expression<Func<Rol, bool>> where);
        IEnumerable<Rol> GetAll();
    }
}
