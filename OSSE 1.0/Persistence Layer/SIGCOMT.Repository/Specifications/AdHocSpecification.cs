using System;
using System.Linq.Expressions;

namespace SIGCOMT.Repository.Specifications
{
    public class AdHocSpecification<T> : QuerySpecification<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        public AdHocSpecification(Expression<Func<T, bool>> expression)
        {
            _expression = expression;
        }

        public override Expression<Func<T, bool>> MatchingCriteria
        {
            get { return _expression; }
        }
    }
}