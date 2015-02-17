using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SIGCOMT.Common.FiltersRules
{
    public class BaseOperationComparison
    {
        public static Expression GetMemberAccessLambda<T>(ParameterExpression arg, string itemField) where T : class
        {
            string[] listaPropiedades = itemField.Split('.');
            Expression expression = arg;

            Type tipoActual = typeof(T);

            foreach (string propiedad in listaPropiedades)
            {
                PropertyInfo propertyInfo = tipoActual.GetProperty(propiedad);
                expression = Expression.MakeMemberAccess(expression, propertyInfo);
                tipoActual = propertyInfo.PropertyType;
            }

            return expression;
        }
    }
}