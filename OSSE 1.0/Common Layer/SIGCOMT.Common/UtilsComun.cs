using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using Newtonsoft.Json;
using SIGCOMT.Common.DataTable;

namespace SIGCOMT.Common
{
    public static class UtilsComun
    {
        /// <summary>
        ///     Metodo para obtener una expresion Lambda para un OrderBy en base al nombre de la propiedad
        /// </summary>
        /// <typeparam name="T">El tipo la clase que contiene la propiedad</typeparam>
        /// <param name="propiedad">El nombre de la propiedad que se usara en el OrderBy</param>
        /// <returns>Una expresion del tipo dynamic</returns>
        public static dynamic LambdaPropertyOrderBy<T>(string propiedad) where T : class
        {
            string[] listaPropiedades = propiedad.Split('.');
            Type type = typeof (T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            foreach (string prop in listaPropiedades)
            {
                PropertyInfo propertyInfo = type.GetProperty(prop);
                expr = Expression.MakeMemberAccess(expr, propertyInfo);
                type = propertyInfo.PropertyType;
            }

            if (type == typeof (string))
            {
                return Expression.Lambda<Func<T, string>>(expr, arg);
            }
            if (type == typeof (int))
            {
                return Expression.Lambda<Func<T, int>>(expr, arg);
            }
            if (type == typeof (decimal))
            {
                return Expression.Lambda<Func<T, decimal>>(expr, arg);
            }
            if (type == typeof (double))
            {
                return Expression.Lambda<Func<T, double>>(expr, arg);
            }
            if (type == typeof (DateTime))
            {
                return Expression.Lambda<Func<T, DateTime>>(expr, arg);
            }
            if (type == typeof (DateTime?))
            {
                return Expression.Lambda<Func<T, DateTime?>>(expr, arg);
            }
            if (type == typeof (float))
            {
                return Expression.Lambda<Func<T, float>>(expr, arg);
            }
            if (type == typeof (bool))
            {
                return Expression.Lambda<Func<T, bool>>(expr, arg);
            }
            if (type == typeof (bool?))
            {
                return Expression.Lambda<Func<T, bool?>>(expr, arg);
            }
            if (type == typeof (int?))
                return Expression.Lambda<Func<T, int?>>(expr, arg);

            throw new Exception("Se debe agregar el tipo " + type.Name + " al metodo LambdaPropertyOrderBy de la clase UtilsComun");
        }

        public static Expression GetMemberAccessLambda<T>(ParameterExpression arg, string itemField) where T : class
        {
            string[] listaPropiedades = itemField.Split('.');
            Expression expression = arg;

            Type tipoActual = typeof (T);

            foreach (string propiedad in listaPropiedades)
            {
                PropertyInfo propertyInfo = tipoActual.GetProperty(propiedad);
                expression = Expression.MakeMemberAccess(expression, propertyInfo);
                tipoActual = propertyInfo.PropertyType;
            }

            return expression;
        }

        /// <summary>
        ///     Permite obtener una expresion lambda
        /// </summary>
        /// <typeparam name="T">Clase de la cual se avalua las propiedades usadas para el filtro</typeparam>
        /// <typeparam name="TQ">Clase que contiene el valor de las propiedades para el filtro</typeparam>
        /// <param name="data">Representa el valor de las propiedades para el filtro</param>
        /// <param name="filterRules">
        ///     Representa la lista de Field-Value, donde "Field" es el nombre de la propiedad de T, y
        ///     "Value" es el nombre de la propiedad de TQ
        /// </param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> ConvertToLambda<T, TQ>(TQ data, Rule[] filterRules)
            where T : class
        {
            Expression<Func<T, bool>> expresionsLambdaSet = null;

            foreach (Rule item in filterRules)
            {
                PropertyInfo propertyKey = null;
                Type tipoActual = typeof (T);

                if (item.Field.Contains("."))
                {
                    #region Seccion para obtener la ultima propiedad de la composicion

                    string[] properties = item.Field.Split('.');

                    foreach (string propiedad in properties)
                    {
                        propertyKey = tipoActual.GetProperty(propiedad);
                        tipoActual = propertyKey.PropertyType;
                    }

                    #endregion
                }
                else
                {
                    propertyKey = typeof (T).GetProperty(item.Field);
                }

                PropertyInfo propertyValue;
                object value = null;

                if (item.Data.Contains("."))
                {
                    #region Seccion para obtener la ultima propiedad de la composicion

                    tipoActual = typeof (TQ);
                    string[] properties = item.Data.Split('.');
                    object valueTemp = data;

                    foreach (string propiedad in properties)
                    {
                        propertyValue = tipoActual.GetProperty(propiedad);
                        tipoActual = propertyValue.PropertyType;
                        value = propertyValue.GetValue(valueTemp);
                        valueTemp = value;
                    }

                    #endregion
                }
                else
                {
                    propertyValue = typeof (TQ).GetProperty(item.Data);
                    value = propertyValue.GetValue(data);
                }

                if (propertyKey != null)
                {
                    Expression valorEvaluar = value == null
                        ? (Expression) Expression.Constant(null)
                        : Expression.Convert(Expression.Constant(Convert.ChangeType(value,
                            Nullable.GetUnderlyingType(propertyKey.PropertyType) ?? propertyKey.PropertyType)),
                            propertyKey.PropertyType);

                    ParameterExpression arg = Expression.Parameter(typeof (T), "p");
                    BinaryExpression comparison = Expression.Equal(GetMemberAccessLambda<T>(arg, item.Field), valorEvaluar);

                    expresionsLambdaSet = expresionsLambdaSet != null
                        ? expresionsLambdaSet.And(Expression.Lambda<Func<T, bool>>(comparison, arg))
                        : Expression.Lambda<Func<T, bool>>(comparison, arg);
                }
            }

            return expresionsLambdaSet ?? PredicateBuilder.True<T>();
        }

        public static Expression<Func<T, bool>> ConvertToLambda<T>(Filter parametro) where T : class
        {
            Expression<Func<T, bool>> expresionsLambdaSet = MergeRules<T>(parametro);

            return expresionsLambdaSet ?? PredicateBuilder.True<T>();
        }

        public static Expression<Func<T, bool>> ConvertToLambda<T>(string filters) where T : class
        {
            Filter parametros = (string.IsNullOrEmpty(filters)) ? null : JsonConvert.DeserializeObject<Filter>(filters);

            if (parametros == null)
                return PredicateBuilder.True<T>();

            Expression<Func<T, bool>> expresionsLambdaSet = MergeRules<T>(parametros);

            return expresionsLambdaSet ?? PredicateBuilder.True<T>();
        }

        public static Expression<Func<T, bool>> ConvertToLambda<T>(List<ColumnModel> columnModels,
            SearchColumn searchColumn, List<ColumnInformation> columnsInformations) where T : class
        {
            if (columnModels == null || searchColumn == null)
                return PredicateBuilder.True<T>();

            Filter parametros = ConvertToFilter(columnModels, searchColumn, columnsInformations);

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

        private static Filter ConvertToFilter(IEnumerable<ColumnModel> columnModels, SearchColumn searchColumn, IEnumerable<ColumnInformation> columnsInformations)
        {
            Filter filtro;
            if (!string.IsNullOrEmpty(searchColumn.Value))
            {
                filtro = new Filter
                {
                    GroupOp = "or",
                    Rules =
                        columnModels.Where(p => p.Searchable).Select(p => CrearRuleColumna(p, searchColumn.Value, columnsInformations)).ToList()
                };
            }
            else
            {
                filtro = new Filter
                {
                    GroupOp = "and",
                    Rules =
                        columnModels.Where(p => !string.IsNullOrEmpty(p.Search.Value) && p.Searchable)
                            .Select(p => CrearRuleColumna(p, p.Search.Value, columnsInformations)).ToList()
                };
            }

            return filtro;
        }

        private static Expression<Func<T, bool>> MergeRules<T>(Filter parametro) where T : class
        {
            Expression<Func<T, bool>> expresionsLambdaSet = null;

            Expression comparison = null;

            foreach (Rule item in parametro.Rules)
            {
                ParameterExpression arg = Expression.Parameter(typeof (T), "p");
                PropertyInfo property;

                if (item.Field.Contains("."))
                {
                    #region Seccion para obtener la ultima propiedad de la composicion

                    string[] properties = item.Field.Split('.');
                    Type tipoActual = typeof (T);
                    PropertyInfo propertyInfo = null;

                    foreach (string propiedad in properties)
                    {
                        propertyInfo = tipoActual.GetProperty(propiedad);
                        tipoActual = propertyInfo.PropertyType;
                    }

                    property = propertyInfo;

                    #endregion
                }
                else
                    property = typeof (T).GetProperty(item.Field);

                if (property != null)
                {
                    TypeConverter converter =
                        TypeDescriptor.GetConverter(Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);

                    bool isValid;

                    if (property.PropertyType == typeof (DateTime))
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
                        ? (Expression) Expression.Constant(item.Data)
                        : Expression.Convert(Expression.Constant(Convert.ChangeType(item.Data,
                            Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType)),
                            property.PropertyType);

                    switch (item.Op)
                    {
                            #region Lista de Expresiones Comparativas

                        case "bw":
                            MethodInfo miBeginWith = typeof (string).GetMethod("StartsWith", new[] {typeof (string)});
                            comparison = Expression.Call(GetMemberAccessLambda<T>(arg, item.Field), miBeginWith, valorEvaluar);
                            break;
                        case "gt":
                            comparison = Expression.GreaterThanOrEqual(GetMemberAccessLambda<T>(arg, item.Field),
                                valorEvaluar);
                            break;
                        case "lt":
                            comparison = Expression.LessThanOrEqual(GetMemberAccessLambda<T>(arg, item.Field),
                                valorEvaluar);
                            break;
                        case "eq":
                            comparison = Expression.Equal(GetMemberAccessLambda<T>(arg, item.Field), valorEvaluar);
                            break;
                        case "ne":
                            comparison = Expression.NotEqual(GetMemberAccessLambda<T>(arg, item.Field), valorEvaluar);
                            break;
                        case "ew":
                            MethodInfo miEndsWith = typeof (string).GetMethod("EndsWith", new[] {typeof (string)});
                            comparison = Expression.Call(GetMemberAccessLambda<T>(arg, item.Field), miEndsWith, valorEvaluar);
                            break;
                        case "cn":
                            MethodInfo miContains = typeof (string).GetMethod("Contains", new[] {typeof (string)});
                            comparison = Expression.Call(GetMemberAccessLambda<T>(arg, item.Field), miContains, valorEvaluar);
                            break;
                        case "fe":
                            break;

                            #endregion
                    }
                }

                #region Concatenacion de las Expresiones de los rules actuales

                if (parametro.GroupOp.ToUpper() == "AND")
                {
                    expresionsLambdaSet = expresionsLambdaSet != null
                        ? expresionsLambdaSet.And(Expression.Lambda<Func<T, bool>>(comparison, arg))
                        : Expression.Lambda<Func<T, bool>>(comparison, arg);
                }
                else if (parametro.GroupOp.ToUpper() == "OR")
                {
                    expresionsLambdaSet = expresionsLambdaSet != null
                        ? expresionsLambdaSet.Or(Expression.Lambda<Func<T, bool>>(comparison, arg))
                        : Expression.Lambda<Func<T, bool>>(comparison, arg);
                }
                else
                    throw new ArgumentException("Argumento GroupOp invalido");

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

        public static string GetExceptionMessage(Exception ex)
        {
            if (ex.InnerException == null)
                return ex.Message;
            string errorMessage = string.Format("{0}\n{1}", ex.Message, GetExceptionMessage(ex.InnerException));
            return errorMessage;
        }

        public static DateTime FechaLimiteEjecucion(DateTime? fechaTentativa, DateTime fechaLimiteEjecucion,
            DateTime fechaInicioEjecucion)
        {
            if (fechaTentativa != null && fechaTentativa.Value.TimeOfDay > fechaLimiteEjecucion.TimeOfDay)
            {
                double factor = 1 - fechaInicioEjecucion.Date.Subtract(fechaTentativa.Value.Date).TotalDays;
                fechaLimiteEjecucion = fechaInicioEjecucion.Date.Add(fechaLimiteEjecucion.TimeOfDay).AddDays(factor);
            }
            else
            {
                if (fechaTentativa != null)
                    fechaLimiteEjecucion = fechaTentativa.Value.Date.Add(fechaLimiteEjecucion.TimeOfDay);
            }

            return fechaLimiteEjecucion;
        }

        public static string GetEnumDescription(System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);

            if (attributes.Length > 0)
                return attributes[0].Description;
            return value.ToString();
        }

        /// <summary>
        ///     Pone nombre al reporte basado en fecha actual
        /// </summary>
        /// <param name="name">Nombre del reporte</param>
        /// <returns></returns>
        public static string GetReporteName(string name)
        {
            return string.Format("{0}_{1}{2}{3}{4}{5}{6}", name, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour,
                DateTime.Now.Minute, DateTime.Now.Second);
        }

        #region Ip y HostName Local

        public static string IpCliente
        {
            get
            {
                IPAddress lastOrDefault =
                    Dns.GetHostAddresses(Dns.GetHostName())
                        .LastOrDefault(p => p.AddressFamily == AddressFamily.InterNetwork);

                return lastOrDefault != null ? lastOrDefault.ToString() : string.Empty;
            }
        }

        public static string HostName
        {
            get { return Dns.GetHostName(); }
        }

        #endregion

        #region Métodos adicionales y extensiones

        #region Extensiones fecha

        /// <summary>
        ///     Convierte una fecha en una cadena con formato: dd/mm/yyyy.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns>dd/mm/yyyy</returns>
        public static string GetDate(this DateTime fecha)
        {
            return string.Format("{0:dd/MM/yyyy}", fecha);
        }

        /// <summary>
        ///     Extrae la hora de una fecha en formato: hh:mm.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns>hh:mm</returns>
        public static string ConvertToHour(this DateTime fecha)
        {
            return string.Format("{0:HH:mm}", fecha);
        }

        /// <summary>
        ///     Convierte una fecha en una cadena con formato: dd/mm/yyyy hh:mm:ss(includeSeconds = true), dd/mm/yyyy hh:mm
        ///     (includeSeconds = false).
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="includeSeconds"></param>
        /// <returns>dd/mm/yyyy hh:mm:ss => (includeSeconds = true), dd/mm/yyyy hh:mm => (includeSeconds = false)</returns>
        public static string GetDateTime(this DateTime dateTime, bool includeSeconds = true)
        {
            return includeSeconds
                ? string.Format("{0:dd/MM/yyyy HH:mm:ss}", dateTime)
                : string.Format("{0:dd/MM/yyyy HH:mm}", dateTime);
        }

        /// <summary>
        ///     Retorna la fecha con la última hora del dia: dd/mm/yy 23:59:59.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static DateTime GetDateEnd(this DateTime fecha)
        {
            return new DateTime(fecha.Year, fecha.Month, fecha.Day, 23, 59, 59);
        }

        /// <summary>
        ///     Retorna la fecha con la hora inicial del dia: dd/mm/yy 0:0:0.
        /// </summary>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static DateTime GetDateStar(this DateTime fecha)
        {
            return new DateTime(fecha.Year, fecha.Month, fecha.Day, 0, 0, 0);
        }

        /// <summary>
        ///     Retorna la fecha equivalente en base al timezone origen hacia el timezone local
        /// </summary>
        /// <param name="dateAnotherTimeZone">fecha en otro timezone distinto al local</param>
        /// <param name="timeZoneSourceId">Id del timezone en el que viene la fecha</param>
        /// <returns>Fecha equivalente en el timezone local</returns>
        public static DateTime ConvertDateTimeToLocalTimeZone(DateTime dateAnotherTimeZone, string timeZoneSourceId)
        {
            DateTime fechaConvertida = TimeZoneInfo.ConvertTime(dateAnotherTimeZone,
                TimeZoneInfo.FindSystemTimeZoneById(timeZoneSourceId),
                TimeZoneInfo.Local);
            return fechaConvertida;
        }

        #endregion

        #region Extensiones enumeración

        public static string GetStringValue(this System.Enum value)
        {
            return Convert.ToString(Convert.ChangeType(value, value.GetTypeCode()));
        }

        public static int GetNumberValue(this System.Enum value)
        {
            return Convert.ToInt32(Convert.ChangeType(value, value.GetTypeCode()));
        }

        #endregion

        #endregion
    }
}