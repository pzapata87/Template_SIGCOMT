using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SIGCOMT.Common;
using SIGCOMT.Resources;

namespace SIGCOMT.Web.Core
{
    public static class Utils
    {
        #region Manejo de URLs

        private static string _relativeWebRoot;

        /// <summary>
        ///     Retorna la ruta relativa al sitio
        /// </summary>
        public static string RelativeWebRoot
        {
            get { return _relativeWebRoot ?? (_relativeWebRoot = VirtualPathUtility.ToAbsolute("~/")); }
        }

        /// <summary>
        ///     Retorna la ruta absoluta al sitio
        /// </summary>
        public static Uri AbsoluteWebRoot
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context == null)
                    throw new WebException("El actual HttpContext es nulo");

                if (context.Items["absoluteurl"] == null)
                    context.Items["absoluteurl"] = new Uri(context.Request.Url.GetLeftPart(UriPartial.Authority) + RelativeWebRoot);

                return context.Items["absoluteurl"] as Uri;
            }
        }

        #endregion Manejo de URLs

        #region Manejo de datos

        public static List<SelectListItem> ConvertToListItem<T>(IList<T> list, string value, string text)
        {
            List<SelectListItem> listItems = (from entity in list
                                              let propiedad1 = entity.GetType().GetProperty(value)
                                              where propiedad1 != null
                                              let valor1 = propiedad1.GetValue(entity, null)
                                              where valor1 != null
                                              let propiedad2 = entity.GetType().GetProperty(text)
                                              where propiedad2 != null
                                              let valor2 = propiedad2.GetValue(entity, null)
                                              where valor2 != null
                                              select new SelectListItem
                                              {
                                                  Value = valor1.ToString(),
                                                  Text = valor2.ToString()
                                              })
                .OrderBy(p => p.Text)
                .ToList();
            listItems.Insert(0, new SelectListItem { Text = @"-- " + Master.Seleccionar + @" --", Value = "0" });
            return listItems;
        }

        public static List<KeyValue> EnumToList<T>()
        {
            Type enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T debe ser de tipo System.Enum");

            Array enumValArray = Enum.GetValues(enumType);
            List<KeyValue> enumValList = (from object l in enumValArray
                                          select new KeyValue
                                          {
                                              Valor = (int)l,
                                              Nombre = Enum.GetName(enumType, l)
                                          })
                .OrderBy(p => p.Nombre)
                .ToList();

            return enumValList;
        }

        #endregion Manejo de datos
    }
}