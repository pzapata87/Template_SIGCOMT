using System;
using System.Linq;
using System.Linq.Expressions;
using OSSE.Common;

namespace OSSE.BusinessLogic.Core
{
    public interface IJQGridPaging<T> where T : class
    {
        int Count(Expression<Func<T, bool>> @where);
        IQueryable<T> GetAll(JQGridParameters<T> parameters);
    }
}
