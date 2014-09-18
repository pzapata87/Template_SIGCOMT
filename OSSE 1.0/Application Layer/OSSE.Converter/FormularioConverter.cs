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

            foreach (var modulo in formularioDomain)
            {
                if (modulo.FormularioParentId.HasValue) continue;

                var idioma = ObtenerIdiomaFormulario(idiomaId, modulo.ItemTablaFormularioList);
                cadenaMenu.AppendLine(GenerarHtmlContenidoModulo(modulo, idioma.Nombre, idiomaId));
            }

            return cadenaMenu.AppendLine("</ul>").ToString();
        }

        #region Metodos Privados GenerateTreeView

        private static string GenerarHtmlContenidoModulo(Formulario modulo, string idiomaNombre, int idiomaId)
        {
            const string patternModulo = @"
                    <li data-idx='{0}' style='display: block'>
                        <a href='javascript:void(0)' title='{1}' class='nodo'>
                            <img src='{2}' alt='' /><span>{1}</span>
                        </a>
                        <div style='display: none'>
                            <ul id='MOD{0}' class='menuNavegacion'>
                                <li><span style='border-top: 0 !important'>{1}
                                    <ul>
                                        {3}
                                    </ul>
                                </li>
                            </ul> 
                        </div>
                    </li>";

            return string.Format(patternModulo, modulo.Id, idiomaNombre, modulo.Direccion,
                GenerateChildren(modulo.FormulariosHijosList, idiomaId));
        }

        private static ItemTablaFormulario ObtenerIdiomaFormulario(int idiomaId, ICollection<ItemTablaFormulario> itemTablaFormularios)
        {
            return itemTablaFormularios.FirstOrDefault(p => p.ItemTablaId == idiomaId) ??
                   itemTablaFormularios.First();
        }

        private static string GenerarHtmlHijos(ICollection<Formulario> formulariosHijos, int idiomaId, string idiomaNombre)
        {
            const string cadenaHija = @"
                            <li><span>{0}</span>
                                <ul>
                                    {1}
                                </ul>
                            </li>";

            return string.Format(cadenaHija, idiomaNombre, GenerateChildren(formulariosHijos, idiomaId));
        }

        private static string GenerateChildren(IEnumerable<Formulario> childrenList, int idiomaId)
        {
            var sb = new StringBuilder();

            foreach (var children in childrenList)
            {
                var idioma = ObtenerIdiomaFormulario(idiomaId, children.ItemTablaFormularioList);
                sb.AppendLine(ProcesarChildren(idiomaId, children, idioma.Nombre));
            }

            return sb.ToString();
        }

        private static string ProcesarChildren(int idiomaId, Formulario children, string idiomaNombre)
        {
            var contenidoHtmlHijo = string.Format(@"
                        <li>
                             <a href='javascript:void(0)' title='{0}' data-url='{1}?id={2}' id='FORM{2}' class='itemMenuClass'>{0}</a>
                        </li>", idiomaNombre, children.Direccion, children.Id);


            if (children.FormulariosHijosList.Any())
            {
                contenidoHtmlHijo = GenerarHtmlHijos(children.FormulariosHijosList, idiomaId, idiomaNombre);
            }

            return contenidoHtmlHijo;
        } 

        #endregion
    }
}
