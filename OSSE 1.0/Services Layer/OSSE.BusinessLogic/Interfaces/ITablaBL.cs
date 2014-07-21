using System;
using System.Linq;
using System.Linq.Expressions;
using OSSE.BusinessLogic.Core;
using OSSE.Domain;

namespace OSSE.BusinessLogic.Interfaces
{
    public interface ITablaBL : IJQGridPaging<Tabla>
   {
       Tabla Get(Expression<Func<Tabla, bool>> where);
       IQueryable<Tabla> FindAll(Expression<Func<Tabla, bool>> where);
       Tabla GetById(long id);
    }
}
