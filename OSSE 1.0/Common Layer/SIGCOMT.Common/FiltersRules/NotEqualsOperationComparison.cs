using System.Linq.Expressions;

namespace SIGCOMT.Common.FiltersRules
{
    public class NotEqualsOperationComparison : BaseOperationComparison, IOperationComparison
    {
        public Expression GetOperationComparison<T>(ParameterExpression parameterExpression, string itemField, Expression expressionValue)
         where T : class
        {
            return Expression.NotEqual(GetMemberAccessLambda<T>(parameterExpression, itemField), expressionValue);
        }
    }
}