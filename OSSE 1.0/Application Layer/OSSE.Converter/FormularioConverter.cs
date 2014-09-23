using System.Collections.Generic;
using System.Linq;
using System.Text;
using OSSE.Domain;
using OSSE.DTO;

namespace OSSE.Converter
{
    public class FormularioConverter
    {
        public static List<FormularioDto> GenerateTreeView(List<Formulario> formularioDomain, int idiomaId)
        {
            return (from modulo in formularioDomain
                where !modulo.FormularioParentId.HasValue
                let idioma = ObtenerIdiomaFormulario(idiomaId, modulo.ItemTablaFormularioList)
                select new FormularioDto
                {
                    Id = modulo.Id,
                    Icono = modulo.Direccion,
                    Nombre = idioma.Nombre,
                    Operaciones = GenerateChildren(modulo.FormulariosHijosList, idiomaId)
                }).ToList();
        }

        #region Metodos Privados GenerateTreeView

        private static ItemTablaFormulario ObtenerIdiomaFormulario(int idiomaId, ICollection<ItemTablaFormulario> itemTablaFormularios)
        {
            return itemTablaFormularios.FirstOrDefault(p => p.ItemTablaId == idiomaId) ??
                   itemTablaFormularios.First();
        }

        private static List<OperacionDto> GenerateChildren(IEnumerable<Formulario> childrenList, int idiomaId)
        {
            return (from children in childrenList
                let idioma = ObtenerIdiomaFormulario(idiomaId, children.ItemTablaFormularioList)
                select new OperacionDto
                {
                    Controlador = children.Controlador,
                    Nombre = idioma.Nombre,
                    Id = children.Id,
                    Operaciones = GenerateChildren(children.FormulariosHijosList, idiomaId)
                }).ToList();
        }

        #endregion
    }
}
