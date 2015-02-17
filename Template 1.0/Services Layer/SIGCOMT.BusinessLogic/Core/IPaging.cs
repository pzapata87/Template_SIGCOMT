using System;
using System.Linq;
using System.Linq.Expressions;
using SIGCOMT.Common;

namespace SIGCOMT.BusinessLogic.Core
{
    public interface IPaging<T> where T : class
    {
        int Count(Expression<Func<T, bool>> @where);
        IQueryable<T> GetAll(FilterParameters<T> parameters);
    }
}