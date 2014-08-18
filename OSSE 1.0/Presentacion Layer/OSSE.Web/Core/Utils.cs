using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OSSE.Common;
using OSSE.Common.Constantes;
using Resources;

namespace OSSE.Web.Core
{
    public static class Utils
    {
        #region Manejo de URLs

        private static string _relativeWebRoot;

        /// <summary>
        /// Retorna la ruta relativa al sitio
        /// </summary>
        public static string RelativeWebRoot
        {
            get { return _relativeWebRoot ?? (_relativeWebRoot = VirtualPathUtility.ToAbsolute("~/")); }
        }

        /// <summary>
        /// Retorna la ruta absoluta al sitio
        /// </summary>
        public static Uri AbsoluteWebRoot
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context == null)
                    throw new System.Net.WebException("El actual HttpContext es nulo");

                if (context.Items["absoluteurl"] == null)
                    context.Items["absoluteurl"] = new Uri(context.Request.Url.GetLeftPart(UriPartial.Authority) + RelativeWebRoot);

                return context.Items["absoluteurl"] as Uri;
            }
        }

        #endregion Manejo de URLs

        #region Manejo de datos

        public static List<SelectListItem> ConvertToListItem<T>(IList<T> list, string value, string text)
        {
            var listItems = (from entity in list
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

        public static List<Comun> EnumToList<T>()
        {
            var enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T debe ser de tipo System.Enum");

            var enumValArray = Enum.GetValues(enumType);
            var enumValList = (from object l in enumValArray
                               select new Comun
                               {
                                   Valor = l.ToString(),
                                   Nombre = Enum.GetName(enumType, l)
                               })
                .OrderBy(p => p.Nombre)
                .ToList();
            return enumValList;
        }

        public static List<Comun> EnumToListNoOrdered<T>()
        {
            var enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T debe ser de tipo System.Enum");

            var enumValArray = Enum.GetValues(enumType);
            var enumValList = (from object l in enumValArray
                               select new Comun
                               {
                                   Valor = l.ToString(),
                                   Nombre = Enum.GetName(enumType, l)
                               })
                .ToList();
            return enumValList;
        }

        public static List<Comun> EnumToListDescription<T>(bool allowValue = false)
        {
            var enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T debe ser de tipo System.Enum");

            var enumValArray = Enum.GetValues(enumType);
            var enumValList = (from object l in enumValArray
                               select new Comun
                               {
                                   Valor = l.ToString(),
                                   Nombre = UtilsComun.GetEnumDescription((Enum)(object)(T)(object)Convert.ToInt32(l))
                               })
                .OrderBy(p => p.Valor)
                .ToList();
            if (allowValue)
                enumValList.Insert(0, new Comun { Nombre = "-- " + Master.Seleccionar + " --", Valor = "0" });
            return enumValList;
        }

        public static int EnumToListGetIntByNombre<T>(string nombre)
        {
            var enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T debe ser de tipo System.Enum");

            var enumValArray = Enum.GetValues(enumType);
            var enumValList = (from object l in enumValArray
                let name = Enum.GetName(enumType, l)
                where name != null
                select new Comun
                               {
                                   Valor = l.ToString(),
                                   Nombre = name.Replace("_", " ")
                               });
            var id = enumValList.FirstOrDefault(p => p.Nombre == nombre);
            return Int32.Parse(id.Valor);
        }

        public static string EnumToListGetNombre<T>(string id)
        {
            var enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T debe ser de tipo System.Enum");

            var enumValArray = Enum.GetValues(enumType);
            var enumValList = (from object l in enumValArray
                               select new  Comun
                               {
                                   Valor = l.ToString(),
                                   Nombre = Enum.GetName(enumType, l).Replace("_", " ")
                               });
            var tipo = enumValList.FirstOrDefault(p => p.Valor == id);
            return tipo.Nombre.Replace("_", " ");
        }

        public static List<Comun> EnumToListOrderById<T>(bool allowValue)
        {
            var enumType = typeof(T);

            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T debe ser de tipo System.Enum");

            var enumValArray = Enum.GetValues(enumType);
            var enumValList = (from object l in enumValArray
                               select new Comun
                               {
                                   Valor = l.ToString(),
                                   Nombre = Enum.GetName(enumType, l)
                               })
                .OrderBy(p => p.Valor)
                .ToList();
            if (allowValue)
                enumValList.Insert(0, new Comun { Nombre = "-- " + Master.Seleccionar + " --", Valor = "0" });
            return enumValList;
        }

        public static List<Comun> ConvertToComunList<T>(IList<T> list, string value, string text, bool allowValue, int val = MasterConstantes.SeleccionarId)
        {
            var listItems = (from entity in list
                             let propiedad1 = entity.GetType().GetProperty(value)
                             where propiedad1 != null
                             let valor1 = propiedad1.GetValue(entity, null)
                             where valor1 != null
                             let propiedad2 = entity.GetType().GetProperty(text)
                             where propiedad2 != null
                             let valor2 = propiedad2.GetValue(entity, null)
                             where valor2 != null
                             select new Comun
                             {
                                 Valor = valor1.ToString(),
                                 Nombre = valor2.ToString()
                             })
                .OrderBy(p => p.Valor)
                .ToList();

            if (allowValue)
            {
                switch (val)
                {
                    case MasterConstantes.SeleccionarId:
                        listItems.Insert(0, new Comun { Nombre = "-- " + Master.Seleccionar + " --", Valor = "" });
                        break;
                    case MasterConstantes.NingunoId:
                        listItems.Insert(0, new Comun { Nombre = "-- " + Master.Ninguno + " --", Valor = "" });
                        break;
                    case MasterConstantes.TodosId:
                        listItems.Insert(0, new Comun { Nombre = "-- " + Master.Todos + " --", Valor = "" });
                        break;
                }
            }

            return listItems;
        }

        #endregion Manejo de datos
    }
}
