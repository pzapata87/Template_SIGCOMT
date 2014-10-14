using System;
using System.Linq;
using System.Linq.Expressions;
using SIGCOMT.BusinessLogic.Core;
using SIGCOMT.Domain;

namespace SIGCOMT.BusinessLogic.Interfaces
{
    public interface ITablaBL : IPaging<Tabla>
    {
        Tabla Get(Expression<Func<Tabla, bool>> where);
        IQueryable<Tabla> FindAll(Expression<Func<Tabla, bool>> where);
        Tabla GetById(long id);
    }
}