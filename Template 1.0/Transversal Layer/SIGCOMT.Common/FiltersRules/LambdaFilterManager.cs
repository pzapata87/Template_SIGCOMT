using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using SIGCOMT.Common.DataTable;

namespace SIGCOMT.Common.FiltersRules
{
    public static class LambdaFilterManager
    {
        private static readonly Dictionary<string, IOperationComparison> OperationComparisons; 

        static LambdaFilterManager()
        {
            OperationComparisons = new Dictionary<string, IOperationComparison>
            {
                {"bw", new BeginWithOperationComparison()},
                {"gt", new GreatherThanOperationComparison()},
                {"lt", new LowerThanOperationComparison()},
                {"eq", new EqualsOperationComparison()},
                {"ne", new NotEqualsOperationComparison()},
                {"ew", new EndsWithOperationComparison()},
                {"cn", new ContainsOperationComparison()}
            };
        }

        public static Expression<Func<T, bool>> ConvertToLambda<T>(List<ColumnModel> columnModels,
            SearchColumn searchColumn, List<ColumnInformation> columnInformations) where T : class
        {
            if (columnModels == null || searchColumn == null)
                return PredicateBuilder.True<T>();

            Filter parametros = ConvertToFilter(columnModels, searchColumn, columnInformations);
            Expression<Func<T, bool>> expresionsLambdaSet = MergeRules<T>(parametros);

            return expresionsLambdaSet ?? PredicateBuilder.True<T>();
        }

        private static Rule CrearRuleColumna(ColumnModel columna, string valorBusqueda, IEnumerable<ColumnInformation> columnsInformations)
        {
            var nuevaRule = new Rule
            {
                Data = valorBusqueda,
                Field = columna.Name,
                Op = "cn"
            };

            if (columnsInformations == null)
                return nuevaRule;

            var valoresColumna = columnsInformations.FirstOrDefault(p => p.Columna == columna.Name);
            if (valoresColumna == null) return nuevaRule;

            nuevaRule.Op = valoresColumna.Operador;

            var valorHomologacion =
                valoresColumna.Valores.FirstOrDefault(p => p.ValorReal.ToLower().Contains(valorBusqueda.ToLower()));
            if (valorHomologacion == null) return nuevaRule;

            nuevaRule.Data = valorHomologacion.ValorHomologado;

            return nuevaRule;
        }

        private static Filter ConvertToFilter(IEnumerable<ColumnModel> columnModels, SearchColumn searchColumn, IEnumerable<ColumnInformation> columnInformations)
        {
            IEnumerable<ColumnModel> searchableColumnModels;
            string groupFilter;
            bool genericSearch = false;

            if (!string.IsNullOrEmpty(searchColumn.Value))
            {
                groupFilter = "or";
                searchableColumnModels = columnModels.Where(p => p.Searchable);
                genericSearch = true;
            }
            else
            {
                groupFilter = "and";
                searchableColumnModels = columnModels.Where(p => !string.IsNullOrEmpty(p.Search.Value) && p.Searchable);
            }

            return new Filter
            {
                GroupOp = groupFilter,
                Rules = searchableColumnModels
                    .Select(p => CrearRuleColumna(p, genericSearch ? searchColumn.Value : p.Search.Value, columnInformations)).ToList()
            };
        }

        private static Expression<Func<T, bool>> MergeRules<T>(Filter parametro) where T : class
        {
            Expression<Func<T, bool>> expresionsLambdaSet = null;

            Expression comparison = null;

            foreach (Rule item in parametro.Rules)
            {
                ParameterExpression arg = Expression.Parameter(typeof(T), "p");
                PropertyInfo property = GetPropertyInfo(typeof(T), item.Field);

                if (property != null)
                {
                    TypeConverter converter =
                        TypeDescriptor.GetConverter(Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);

                    bool isValid;

                    if (property.PropertyType == typeof(DateTime))
                    {
                        DateTime fechaCasteada;
                        isValid = DateTime.TryParse(item.Data, out fechaCasteada);
                    }
                    else
                    {
                        isValid = converter.IsValid(item.Data);
                    }

                    if (!isValid)
                        continue;

                    Expression valorEvaluar = item.Data == null
                        ? (Expression)Expression.Constant(item.Data)
                        : Expression.Convert(Expression.Constant(Convert.ChangeType(item.Data,
                            Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType)),
                            property.PropertyType);

                    comparison = OperationComparisons[item.Op].GetOperationComparison<T>(arg, item.Field, valorEvaluar);
                }

                #region Concatenacion de las Expresiones de los rules actuales

                if (expresionsLambdaSet == null)
                    expresionsLambdaSet = Expression.Lambda<Func<T, bool>>(comparison, arg);

                else
                {
                    if (parametro.GroupOp.ToUpper() == "AND")
                    {
                        expresionsLambdaSet = expresionsLambdaSet.And(Expression.Lambda<Func<T, bool>>(comparison, arg));
                    }
                    else if (parametro.GroupOp.ToUpper() == "OR")
                    {
                        expresionsLambdaSet = expresionsLambdaSet.Or(Expression.Lambda<Func<T, bool>>(comparison, arg));
                    }
                    else
                        throw new ArgumentException("Argumento GroupOp invalido");

                }

                #endregion
            }

            if (parametro.Groups == null)
                return expresionsLambdaSet;

            #region Manejo de Expresiones hijas de esta expresion

            foreach (Filter parametroHijo in parametro.Groups)
            {
                Expression<Func<T, bool>> expressionHijo = MergeRules<T>(parametroHijo);
                if (expressionHijo == null) continue;

                if (expresionsLambdaSet == null)
                {
                    expresionsLambdaSet = expressionHijo;
                    continue;
                }

                if (parametro.GroupOp.ToUpper() == "AND")
                {
                    expresionsLambdaSet = expresionsLambdaSet.And(expressionHijo);
                }
                else if (parametro.GroupOp.ToUpper() == "OR")
                {
                    expresionsLambdaSet = expresionsLambdaSet.Or(MergeRules<T>(parametroHijo));
                }
                else
                    throw new ArgumentException("Argumento GroupOp invalido");
            }

            #endregion

            return expresionsLambdaSet;
        }

        private static PropertyInfo GetPropertyInfo(Type tipoDato, string field)
        {
            PropertyInfo property;

            if (field.Contains("."))
            {
                #region Seccion para obtener la ultima propiedad de la composicion

                string[] properties = field.Split('.');
                PropertyInfo propertyInfo = null;

                foreach (string propiedad in properties)
                {
                    propertyInfo = tipoDato.GetProperty(propiedad);
                    tipoDato = propertyInfo.PropertyType;
                }

                property = propertyInfo;

                #endregion
            }
            else
                property = tipoDato.GetProperty(field);

            return property;
        }
    }
}