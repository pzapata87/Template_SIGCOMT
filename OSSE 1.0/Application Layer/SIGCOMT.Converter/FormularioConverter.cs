using System.Collections.Generic;
using System.Linq;
using OSSE.Common.Enum;
using OSSE.Domain;
using OSSE.DTO;
using System;

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

        public static PermisoFormularioDto ObtenerPermisosFormulario(Formulario formulario, IEnumerable<PermisoRol> permisos)
        {
            var permisoFormulario = new PermisoFormularioDto();

            foreach (PermisoRol permiso in permisos)
            {
                var tipoPermiso = (TipoPermiso)Enum.Parse(typeof(TipoPermiso), permiso.TipoPermiso.ToString());
                AsignarPermisoAPropiedad(permisoFormulario, tipoPermiso);
            }

            return permisoFormulario;
        }

        private static void AsignarPermisoAPropiedad(PermisoFormularioDto permisoFormularioDto, TipoPermiso permiso)
        {
            switch (permiso)
            {
                case TipoPermiso.Mostrar:
                    permisoFormularioDto.Mostrar = true;
                    break;
                case TipoPermiso.Crear:
                    permisoFormularioDto.Crear = true;
                    break;
                case TipoPermiso.Modificar:
                    permisoFormularioDto.Modificar = true;
                    break;
                case TipoPermiso.Eliminar:
                    permisoFormularioDto.Eliminar = true;
                    break;
                case TipoPermiso.Imprimir:
                    permisoFormularioDto.Imprimir = true;
                    break;
                case TipoPermiso.Mover:
                    permisoFormularioDto.Mover = true;
                    break;
                case TipoPermiso.Reportar:
                    permisoFormularioDto.Reportar = true;
                    break;
            }
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
                    Controlador = children.Direccion,
                    Nombre = idioma.Nombre,
                    Id = children.Id,
                    Operaciones = GenerateChildren(children.FormulariosHijosList, idiomaId)
                }).ToList();
        }

        #endregion
    }
}