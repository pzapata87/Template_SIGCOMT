using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSSE.Domain;

namespace OSSE.Converter
{
    public class FormularioConverter
    {
        public static string GenerateTreeView(List<Formulario> formularioDomain, int idiomaId)
        {
            var cadenaMenu = new StringBuilder("<ul id='ModulosSIG'>");

            foreach (var modulo in formularioDomain.Where(p => !p.FormularioParentId.HasValue))
            {
                var idioma = modulo.ItemTablaFormularioList.FirstOrDefault(p => p.ItemTablaId == idiomaId) ??
                             modulo.ItemTablaFormularioList.First();

                cadenaMenu.AppendLine("<li data-idx='" + modulo.Id + "' style='display: block'><a href='javascript:void(0)' title='" + idioma.Nombre + "' class='nodo'>");
                cadenaMenu.AppendLine("<img src='" + modulo.Direccion + "' alt='' /><span>" + idioma.Nombre + "</span></a>");
                cadenaMenu.AppendLine("<div style='display: none'>");

                cadenaMenu.AppendFormat("<ul id='{0}' class='{1}'>\r\n", string.Format("MOD{0}", modulo.Id), "menuNavegacion");
                cadenaMenu.AppendFormat("<li><span style='border-top: 0 !important'>{0}", idioma.Nombre.ToUpper());

                cadenaMenu.AppendFormat("<ul>\r\n");

                GenerateChildren(cadenaMenu, modulo.FormulariosHijosList, idiomaId);

                cadenaMenu.AppendFormat("</ul>\r\n");

                cadenaMenu.AppendLine("</li></ul>");
                cadenaMenu.AppendLine("</div></li>");
            }
            cadenaMenu.AppendLine("</ul>");

            return cadenaMenu.ToString();
        }

        private static void GenerateChildren(StringBuilder sb, IEnumerable<Formulario> childrenList, int idiomaId)
        {
            foreach (var children in childrenList)
            {
                var idioma = children.ItemTablaFormularioList.FirstOrDefault(p => p.ItemTablaId == idiomaId) ??
                                children.ItemTablaFormularioList.First();

                if (children.FormulariosHijosList.Any())
                {
                    sb.AppendFormat("<li>{0}", string.Format("<span>{0}</span>", idioma.Nombre));
                    sb.AppendLine("\r\n<ul>");

                    GenerateChildren(sb, children.FormulariosHijosList, idiomaId);

                    sb.AppendLine("</ul></li>");
                }
                else
                {
                    sb.AppendFormat("<li>{0}</li>",
                        string.Format(
                            "<a href='javascript:void(0)' title='{0}' data-url='{1}?id={2}' id='FORM{2}' class='itemMenuClass'>{0}</a>",
                            idioma.Nombre, children.Direccion, children.Id));
                }
            }
        }
    }
}
