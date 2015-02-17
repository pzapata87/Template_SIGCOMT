using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace SIGCOMT.Common
{
    public static class Helper
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
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;

            foreach (string prop in listaPropiedades)
            {
                PropertyInfo propertyInfo = type.GetProperty(prop);
                expr = Expression.MakeMemberAccess(expr, propertyInfo);
                type = propertyInfo.PropertyType;
            }

            return Expression.Lambda(expr, arg);
        }

        public static string GetExceptionMessage(Exception ex)
        {
            if (ex.InnerException == null)
                return ex.Message;
            string errorMessage = string.Format("{0}\n{1}", ex.Message, GetExceptionMessage(ex.InnerException));

            return errorMessage;
        }

        public static string GetEnumDescription(System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

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

        #region Métodos adicionales y de extensión para fechas

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
    }
}