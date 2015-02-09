using System.Linq.Expressions;

namespace SIGCOMT.Common.FiltersRules
{
    public interface IOperationComparison
    {
        Expression GetOperationComparison<T>(ParameterExpression parameterExpression, string itemField, Expression expressionValue)
            where T : class;
    }
}