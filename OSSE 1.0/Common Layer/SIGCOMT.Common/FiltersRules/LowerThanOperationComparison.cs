using System.Linq.Expressions;

namespace SIGCOMT.Common.FiltersRules
{
    public class LowerThanOperationComparison : BaseOperationComparison, IOperationComparison
    {
        public Expression GetOperationComparison<T>(ParameterExpression parameterExpression, string itemField, Expression expressionValue)
           where T : class
        {
            return Expression.LessThanOrEqual(GetMemberAccessLambda<T>(parameterExpression, itemField), expressionValue);
        }
    }
}