using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using SIGCOMT.Common;
using SIGCOMT.Common.Enum;
using SIGCOMT.Domain;
using SIGCOMT.DTO;
using SIGCOMT.Resources;

namespace SIGCOMT.Converter
{
    public class FormularioConverter
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager("SIGCOMT.Resources.Master", typeof(Master).Assembly);

        public static List<ModuloDto> GenerateTreeView(List<Formulario> formularioDomain, int idiomaId)
        {
            return (from modulo in formularioDomain
                    where !modulo.FormularioParentId.HasValue && modulo.Estado == TipoEstado.Activo.GetNumberValue()
                    select new ModuloDto
                    {
                        Id = modulo.Id,
                        Icono = modulo.Direccion,
                        Nombre = ResourceManager.GetString(modulo.ResourceKey),
                        Operaciones = GenerateChildren(modulo.FormulariosHijosList)
                    }).ToList();
        }

        public static PermisoFormularioDto ObtenerPermisosFormulario(Formulario formulario, IEnumerable<PermisoFormularioRol> permisos)
        {
            var permisoFormulario = new PermisoFormularioDto();

            foreach (var permiso in permisos)
            {
                var tipoPermiso = (TipoPermiso) Enum.Parse(typeof (TipoPermiso), Convert.ToString(permiso.TipoPermiso));
                AsignarPermisoAPropiedad(permisoFormulario, tipoPermiso);
            }

            return permisoFormulario;
        }

        public static List<FormularioDto> DomainToDtoFormulario(List<Formulario> formularios, int idiomaId)
        {
            return (from modulo in formularios
                    where !modulo.FormularioParentId.HasValue
                    let nombreModulo = ResourceManager.GetString(modulo.ResourceKey)
                    from formulario in modulo.FormulariosHijosList
                    where formulario.Estado == TipoEstado.Activo.GetNumberValue()
                    select new FormularioDto
                    {
                        Id = formulario.Id,
                        Modulo = ResourceManager.GetString(modulo.ResourceKey),
                        Nombre = ResourceManager.GetString(formulario.ResourceKey),
                        PermisoList = ObtenerPermisosFormulario(formulario.PermisoList)
                    }).ToList();
        }

        public static List<PermisoFormularioDto> ObtenerPermisosFormulario(IEnumerable<PermisoFormulario> permisos)
        {
            return permisos.Select(p => new PermisoFormularioDto
            {
                FormularioId = p.FormularioId,
                TipoPermiso = p.TipoPermiso,
                NombrePermiso = ResourceManager.GetString(Enum.GetName(typeof(TipoPermiso), p.TipoPermiso))
            }).ToList();
        }

        #region Métodos Privados

        private static List<OperacionDto> GenerateChildren(IEnumerable<Formulario> childrenList)
        {
            return (from children in childrenList
                    where children.Estado == TipoEstado.Activo.GetNumberValue()
                    select new OperacionDto
                    {
                        Controlador = children.Direccion,
                        Nombre = ResourceManager.GetString(children.ResourceKey),
                        Id = children.Id,
                        Operaciones = GenerateChildren(children.FormulariosHijosList)
                    }).ToList();
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
                case TipoPermiso.Editar:
                    permisoFormularioDto.Modificar = true;
                    break;
                case TipoPermiso.Eliminar:
                    permisoFormularioDto.Eliminar = true;
                    break;
            }
        }

        #endregion
    }
}