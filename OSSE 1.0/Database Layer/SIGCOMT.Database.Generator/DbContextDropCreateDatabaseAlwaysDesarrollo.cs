using System.Collections.Generic;
using System.Data.Entity;
using SIGCOMT.Common;
using SIGCOMT.Common.Constantes;
using SIGCOMT.Common.Enum;
using SIGCOMT.Domain;
using SIGCOMT.Persistence.EntityFramework;

namespace SIGCOMT.DataBase.Generator
{
    public class DbContextDropCreateDatabaseAlwaysDesarrollo : DropCreateDatabaseAlways<DbContextBase>
    {
        private ItemTabla _idiomaEspañol;
        private Rol _rolAdmin;

        protected override void Seed(DbContextBase context)
        {
            List<ItemTabla> idiomas = AgregarRegistrosTabla(context);
            List<Rol> roles = AgregarRegistrosRol(context);

            _rolAdmin = roles[0];
            _idiomaEspañol = idiomas[0];

            AgregarRegistrosUsuario(context);
            AgregarRegistrosModulo(context);
        }

        private List<ItemTabla> AgregarRegistrosTabla(DbContext context)
        {
            var idiomaEspañol = new ItemTabla
            {
                Nombre = "es-PE",
                Descripcion = "Español",
                Estado = TipoEstado.Activo.GetNumberValue(),
                Valor = "1"
            };

            var idiomas = new List<ItemTabla> {idiomaEspañol};
            int activo = TipoEstado.Activo.GetNumberValue();

            var listaTablas = new List<Tabla>
            {
                new Tabla
                {
                    Id = TipoTabla.Idioma.GetNumberValue(),
                    Nombre = "Idioma",
                    Descripcion = string.Empty,
                    Estado = activo,
                    ItemTabla = idiomas
                },
                new Tabla
                {
                    Id = TipoTabla.TipoEstado.GetNumberValue(),
                    Nombre = "Estado",
                    Descripcion = string.Empty,
                    Estado = activo,
                    ItemTabla = new List<ItemTabla>
                    {
                        new ItemTabla
                        {
                            Nombre = "Inactivo",
                            Descripcion = string.Empty,
                            Estado = activo,
                            Valor = TipoEstado.Inactivo.GetStringValue()
                        },
                        new ItemTabla
                        {
                            Nombre = "Activo",
                            Descripcion = string.Empty,
                            Estado = activo,
                            Valor = TipoEstado.Activo.GetStringValue()
                        }
                    }
                },
                new Tabla
                {
                    Id = TipoTabla.TipoPermiso.GetNumberValue(),
                    Nombre = "Permiso",
                    Descripcion = string.Empty,
                    Estado = activo,
                    ItemTabla = new List<ItemTabla>
                    {
                        new ItemTabla
                        {
                            Nombre = "Mostrar",
                            Descripcion = string.Empty,
                            Estado = activo,
                            Valor = TipoPermiso.Mostrar.GetStringValue()
                        },
                        new ItemTabla
                        {
                            Nombre = "Crear",
                            Descripcion = string.Empty,
                            Estado = activo,
                            Valor = TipoPermiso.Crear.GetStringValue()
                        },
                        new ItemTabla
                        {
                            Nombre = "Modificar",
                            Descripcion = string.Empty,
                            Estado = activo,
                            Valor = TipoPermiso.Modificar.GetStringValue()
                        },
                        new ItemTabla
                        {
                            Nombre = "Eliminar",
                            Descripcion = string.Empty,
                            Estado = activo,
                            Valor = TipoPermiso.Eliminar.GetStringValue()
                        }
                    }
                }
            };
            
            context.Set<Tabla>().AddRange(listaTablas);
            return idiomas;
        }

        private List<Rol> AgregarRegistrosRol(DbContext context)
        {
            var roles = new List<Rol>
            {
                new Rol {Nombre = "Administrador", Estado = TipoEstado.Activo.GetNumberValue()}
            };

            context.Set<Rol>().AddRange(roles);
            return roles;
        }

        private void AgregarRegistrosUsuario(DbContext context)
        {
            int activo = TipoEstado.Activo.GetNumberValue();

            var usuario = new Usuario
            {
                UserName = "Admin",
                Password = "x1z4IYAEC3Q2LZzLIC5f5g==",
                Email = "admin@sigcomt.com",
                Estado = activo,
                IdiomaId = int.Parse(_idiomaEspañol.Valor),
                RolUsuarioList = new[]
                {
                    new RolUsuario {Rol = _rolAdmin, Estado = activo}
                }
            };

            context.Set<Usuario>().Add(usuario);
        }

        private void AgregarRegistrosModulo(DbContext context)
        {
            int activo = TipoEstado.Activo.GetNumberValue();

            #region Titulo/Imagen Por Módulo

            var moduloSeguridad = new Formulario
            {
                Direccion = IconosSvgConstantes.ModuloSeguridad,
                Controlador = string.Empty,
                Orden = 0,
                Nivel = 0,
                FormularioParentId = null,
                Estado = activo,
                PermisoRolList = new List<PermisoFormularioRol>
                {
                    new PermisoFormularioRol {TipoPermiso = 1, Rol = _rolAdmin, Activo = true, Estado = activo},
                },
                ItemTablaFormularioList = new List<ItemTablaFormulario>
                {
                    new ItemTablaFormulario
                    {
                        ItemTabla = _idiomaEspañol,
                        Estado = activo,
                        Nombre = "Seguridad"
                    }
                }
            };

            context.Set<Formulario>().Add(moduloSeguridad);

            #endregion

            #region Mantenedores Mantenimientos Generales

            var grupoVistasModuloSeguridad = new List<Formulario>
            {  
                #region Módulo Seguridad

                MantenedorUsuario(moduloSeguridad),
                MantenedorFormulario(moduloSeguridad)

                #endregion
            };

            context.Set<Formulario>().AddRange(grupoVistasModuloSeguridad);

            #endregion
        }

        private Formulario MantenedorUsuario(Formulario parent)
        {
            int activo = TipoEstado.Activo.GetNumberValue();

            var formulario = new Formulario
            {
                Direccion = "/Administracion/Usuario/Index",
                Controlador = "usuarioList",
                FormularioParent = parent,
                Orden = 1,
                Nivel = 1,
                Estado = activo,
                PermisoRolList = new List<PermisoFormularioRol>
                {
                    new PermisoFormularioRol
                    {
                        TipoPermiso = TipoPermiso.Crear.GetNumberValue(),
                        Rol = _rolAdmin,
                        Activo = true,
                        Estado = activo
                    },
                    new PermisoFormularioRol
                    {
                        TipoPermiso = TipoPermiso.Modificar.GetNumberValue(),
                        Rol = _rolAdmin,
                        Activo = true,
                        Estado = activo
                    },
                    new PermisoFormularioRol
                    {
                        TipoPermiso = TipoPermiso.Eliminar.GetNumberValue(),
                        Rol = _rolAdmin,
                        Activo = true,
                        Estado = activo
                    }
                },
                ItemTablaFormularioList = new List<ItemTablaFormulario>
                {
                    new ItemTablaFormulario {ItemTabla = _idiomaEspañol, Estado = activo, Nombre = "Usuario"}
                }
            };

            return formulario;
        }

        private Formulario MantenedorFormulario(Formulario parent)
        {
            int activo = TipoEstado.Activo.GetNumberValue();

            var formulario = new Formulario
            {
                Direccion = "/Administracion/Formulario/Index",
                Controlador = "Formulario",
                FormularioParent = parent,
                Orden = 1,
                Nivel = 1,
                Estado = activo,
                PermisoRolList = new List<PermisoFormularioRol>
                {
                    new PermisoFormularioRol
                    {
                        TipoPermiso = TipoPermiso.Crear.GetNumberValue(),
                        Rol = _rolAdmin,
                        Activo = true,
                        Estado = activo
                    },
                    new PermisoFormularioRol
                    {
                        TipoPermiso = TipoPermiso.Modificar.GetNumberValue(),
                        Rol = _rolAdmin,
                        Activo = true,
                        Estado = activo
                    },
                    new PermisoFormularioRol
                    {
                        TipoPermiso = TipoPermiso.Eliminar.GetNumberValue(),
                        Rol = _rolAdmin,
                        Activo = true,
                        Estado = activo
                    }
                },
                ItemTablaFormularioList = new List<ItemTablaFormulario>
                {
                    new ItemTablaFormulario {ItemTabla = _idiomaEspañol, Estado = activo, Nombre = "Formulario"}
                }
            };

            return formulario;
        }
    }
}