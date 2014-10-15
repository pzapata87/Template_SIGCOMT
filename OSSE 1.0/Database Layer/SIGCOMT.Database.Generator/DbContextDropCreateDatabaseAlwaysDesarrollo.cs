using System.Collections.Generic;
using System.Data.Entity;
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
            var idiomaEspañol = new ItemTabla {Nombre = "es-PE", Descripcion = "Español", Estado = (int) TipoEstado.Activo, Valor = "1"};
            var idiomas = new List<ItemTabla> {idiomaEspañol};

            var listaTablas = new List<Tabla>
            {
                new Tabla
                {
                    Id = (int) TipoTabla.Idioma,
                    Nombre = "Idioma",
                    Descripcion = string.Empty,
                    Estado = (int) TipoEstado.Activo,
                    ItemTabla = idiomas
                },
                new Tabla
                {
                    Id = (int) TipoTabla.TipoEstado,
                    Nombre = "Estado",
                    Descripcion = string.Empty,
                    Estado = (int) TipoEstado.Activo,
                    ItemTabla = new List<ItemTabla>
                    {
                        new ItemTabla {Nombre = "Inactivo", Descripcion = string.Empty, Estado = (int) TipoEstado.Activo, Valor = "0"},
                        new ItemTabla {Nombre = "Activo", Descripcion = string.Empty, Estado = (int) TipoEstado.Activo, Valor = "1"}
                    }
                },
                new Tabla
                {
                    Id = (int) TipoTabla.TipoPermiso,
                    Nombre = "Permiso",
                    Descripcion = string.Empty,
                    Estado = (int) TipoEstado.Activo,
                    ItemTabla = new List<ItemTabla>
                    {
                        new ItemTabla {Nombre = "Mostrar", Descripcion = string.Empty, Estado = (int) TipoEstado.Activo, Valor = "1"},
                        new ItemTabla {Nombre = "Crear", Descripcion = string.Empty, Estado = (int) TipoEstado.Activo, Valor = "2"},
                        new ItemTabla {Nombre = "Modificar", Descripcion = string.Empty, Estado = (int) TipoEstado.Activo, Valor = "3"},
                        new ItemTabla {Nombre = "Eliminar", Descripcion = string.Empty, Estado = (int) TipoEstado.Activo, Valor = "4"},
                        new ItemTabla {Nombre = "Imprimir", Descripcion = string.Empty, Estado = (int) TipoEstado.Activo, Valor = "5"},
                        new ItemTabla {Nombre = "Mover", Descripcion = string.Empty, Estado = (int) TipoEstado.Activo, Valor = "6"},
                        new ItemTabla {Nombre = "Reportar", Descripcion = string.Empty, Estado = (int) TipoEstado.Activo, Valor = "7"}
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
                new Rol {Nombre = "Administrador", Estado = (int) TipoEstado.Activo}
            };

            context.Set<Rol>().AddRange(roles);
            return roles;
        }

        private void AgregarRegistrosUsuario(DbContext context)
        {
            var usuario = new Usuario
            {
                UserName = "Admin",
                Password = "x1z4IYAEC3Q2LZzLIC5f5g==",
                Email = "admin@sigcomt.com",
                Estado = (int) TipoEstado.Activo,
                IdiomaId = int.Parse(_idiomaEspañol.Valor),
                RolUsuarioList = new[]
                {
                    new RolUsuario {Rol = _rolAdmin, Estado = (int) TipoEstado.Activo}
                }
            };

            context.Set<Usuario>().Add(usuario);
        }

        private void AgregarRegistrosModulo(DbContext context)
        {
            #region Titulo/Imagen Módulo

            #region Módulo Mantenimientos Generales

            var moduloSeguridad = new Formulario
            {
                Direccion = IconosSvgConstantes.ModuloSeguridad,
                Controlador = string.Empty,
                Orden = 0,
                Nivel = 0,
                FormularioParentId = null,
                Estado = (int) TipoEstado.Activo,
                PermisoRolList = new List<PermisoRol>
                {
                    new PermisoRol {TipoPermiso = 1, Rol = _rolAdmin, Estado = (int) TipoEstado.Activo},
                },
                ItemTablaFormularioList = new List<ItemTablaFormulario>
                {
                    new ItemTablaFormulario
                    {
                        ItemTabla = _idiomaEspañol,
                        Estado = (int) TipoEstado.Activo,
                        Nombre = "Seguridad"
                    }
                }
            };


            context.Set<Formulario>().Add(moduloSeguridad);

            #endregion

            #endregion

            #region Mantenedores Módulo

            #region Mantenedores Mantenimientos Generales

            var grupoVistasModuloSeguridad = new List<Formulario>
            {
                new Formulario
                {
                    Direccion = "~/Administracion/Usuario/Index",
                    Controlador = "usuarioList",
                    FormularioParent = moduloSeguridad,
                    Orden = 1,
                    Nivel = 1,
                    Estado = (int) TipoEstado.Activo,
                    PermisoRolList = new List<PermisoRol>
                    {
                        new PermisoRol {TipoPermiso = 1, Rol = _rolAdmin, Estado = (int) TipoEstado.Activo},
                        new PermisoRol {TipoPermiso = 2, Rol = _rolAdmin, Estado = (int) TipoEstado.Activo},
                        new PermisoRol {TipoPermiso = 3, Rol = _rolAdmin, Estado = (int) TipoEstado.Activo},
                        new PermisoRol {TipoPermiso = 4, Rol = _rolAdmin, Estado = (int) TipoEstado.Activo}
                    },
                    ItemTablaFormularioList = new List<ItemTablaFormulario>
                    {
                        new ItemTablaFormulario {ItemTabla = _idiomaEspañol, Estado = (int) TipoEstado.Activo, Nombre = "Usuario"}
                    }
                }
            };

            context.Set<Formulario>().AddRange(grupoVistasModuloSeguridad);

            #endregion

            #endregion
        }
    }
}