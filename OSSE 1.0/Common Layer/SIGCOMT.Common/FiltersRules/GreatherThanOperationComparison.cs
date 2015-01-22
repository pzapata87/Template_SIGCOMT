using System.Linq.Expressions;

namespace SIGCOMT.Common.FiltersRules
{
    public class GreatherThanOperationComparison : BaseOperationComparison, IOperationComparison
    {
        public Expression GetOperationComparison<T>(ParameterExpression parameterExpression, string itemField, Expression expressionValue)
           where T : class
        {
            return Expression.GreaterThanOrEqual(GetMemberAccessLambda<T>(parameterExpression, itemField), expressionValue);
        }
    }
}