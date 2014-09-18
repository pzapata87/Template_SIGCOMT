using System.Collections.Generic;
using System.Data.Entity;
using OSSE.Common.Enum;
using OSSE.Domain;
using OSSE.Persistence.EntityFramework;

namespace OSSE.DataBase.Generator
{
    public class DbContextDropCreateDatabaseAlwaysDesarrollo : DropCreateDatabaseAlways<DbContextBase>
    {
        protected override void Seed(DbContextBase context)
        {
            AgregarRegistrosTabla(context);
            AgregarRegistrosRol(context);
            AgregarRegistrosUsuario(context);
            AgregarRegistrosModulo(context);

            //context.SaveChanges();
        }

        private static void AgregarRegistrosTabla(DbContext context)
        {
            var listaTablas = new List<Tabla>
            {
                new Tabla
                {
                    Id = (int) TipoTabla.Idioma,
                    Nombre = "Idioma",
                    Descripcion = string.Empty,
                    Estado = 1,
                    ItemTabla = new List<ItemTabla>
                    {
                        new ItemTabla { Nombre = "es-PE", Descripcion = "Español", Estado = 1, Valor = "1" },
                        new ItemTabla { Nombre = "en-US", Descripcion = "Ingles", Estado = 1, Valor = "2" }
                    }
                },
                new Tabla
                {
                    Id = (int) TipoTabla.TipoEstado,
                    Nombre = "Estado",
                    Descripcion = string.Empty,
                    Estado = 1,
                    ItemTabla = new List<ItemTabla>
                    {
                        new ItemTabla {Nombre = "Inactivo", Descripcion = string.Empty, Estado = 1, Valor = "0"},
                        new ItemTabla {Nombre = "Activo", Descripcion = string.Empty, Estado = 1, Valor = "1"}
                    }
                },
                new Tabla
                {
                    Id = (int) TipoTabla.TipoPermiso,
                    Nombre = "Permiso",
                    Descripcion = string.Empty,
                    Estado = 1,
                    ItemTabla = new List<ItemTabla>
                    {
                        new ItemTabla {Nombre = "Mostrar", Descripcion = string.Empty, Estado = 1, Valor = "1"},
                        new ItemTabla {Nombre = "Crear", Descripcion = string.Empty, Estado = 1, Valor = "2"},
                        new ItemTabla {Nombre = "Modificar", Descripcion = string.Empty, Estado = 1, Valor = "3"},
                        new ItemTabla {Nombre = "Eliminar", Descripcion = string.Empty, Estado = 1, Valor = "4"},
                        new ItemTabla {Nombre = "Imprimir", Descripcion = string.Empty, Estado = 1, Valor = "5"},
                        new ItemTabla {Nombre = "Mover", Descripcion = string.Empty, Estado = 1, Valor = "6"},
                        new ItemTabla {Nombre = "Reportar", Descripcion = string.Empty, Estado = 1, Valor = "7"}
                    }
                }
            };

            context.Set<Tabla>().AddRange(listaTablas);
            context.SaveChanges();
        }

        private static void AgregarRegistrosRol(DbContext context)
        {
            var roles = new List<Rol>
            {
                new Rol {Id = 1, Nombre = "Administrador", Estado = 1},
                new Rol {Id = 2, Nombre = "Estilista", Estado = 1}
            };

            context.Set<Rol>().AddRange(roles);
        }

        private static void AgregarRegistrosUsuario(DbContext context)
        {
            var usuario = new Usuario
            {
                Id = 1,
                UserName = "Admin",
                Password = "x1z4IYAEC3Q2LZzLIC5f5g==",
                Email = "admin@sigcomt.com",
                Estado = 1,
                IdiomaId = 1,
                RolUsuarioList = new[]
                {
                    new RolUsuario { RolId = 1, Estado = 1}
                }
            };

            context.Set<Usuario>().Add(usuario);
        }

        private static void AgregarRegistrosModulo(DbContext context)
        {
            #region Titulo/Imagen Módulo

            #region Módulo Mantenimientos Generales

            var moduloMantenimientosGenerales = new Formulario
            {
                Direccion = "Content/crm/images/editar.png",
                Controlador = string.Empty,
                Orden = 0,
                Nivel = 0,
                FormularioParentId = null,
                Estado = 1,
                PermisoRolList = new List<PermisoRol>
                {
                    new PermisoRol {TipoPermiso = 1, RolId = 1, Estado = 1}
                },
                ItemTablaFormularioList = new List<ItemTablaFormulario>
                {
                    new ItemTablaFormulario {ItemTablaId = 1, Estado = 1, Nombre = "Mantenimientos Generales"}
                }
            };

            var moduloMantenimientoTest = new Formulario
            {
                Direccion = "Content/crm/images/editar.png",
                Controlador = string.Empty,
                Orden = 0,
                Nivel = 0,
                FormularioParentId = null,
                Estado = 1,
                PermisoRolList = new List<PermisoRol>
                {
                    new PermisoRol {TipoPermiso = 1, RolId = 1, Estado = 1}
                },
                ItemTablaFormularioList = new List<ItemTablaFormulario>
                {
                    new ItemTablaFormulario {ItemTablaId = 1, Estado = 1, Nombre = "Operaciones"}
                }
            };

            context.Set<Formulario>().Add(moduloMantenimientosGenerales);
            context.Set<Formulario>().Add(moduloMantenimientoTest);

            #endregion

            #endregion

            #region Mantenedores Módulo

            #region Mantenedores Mantenimientos Generales

            var grupoOperacionMantenedores = new Formulario
            {
                Direccion = string.Empty,
                Controlador = string.Empty,
                Orden = 0,
                Nivel = 1,
                FormularioParent = moduloMantenimientosGenerales,
                Estado = 1,
                PermisoRolList = new List<PermisoRol>
                {
                    new PermisoRol {TipoPermiso = 1, RolId = 1, Estado = 1}
                },
                ItemTablaFormularioList = new List<ItemTablaFormulario>
                {
                    new ItemTablaFormulario {ItemTablaId = 1, Estado = 1, Nombre = "Mantenedores"}
                },
                FormulariosHijosList = new List<Formulario>
                {
                    new Formulario
                    {
                        Direccion = "Administracion/Usuario/Index",
                        Controlador = "usuarioList",
                        Orden = 1,
                        Nivel = 2,
                        Estado = 1,
                        PermisoRolList = new List<PermisoRol>
                        {
                            new PermisoRol {TipoPermiso = 1, RolId = 1, Estado = 1},
                            new PermisoRol {TipoPermiso = 2, RolId = 1, Estado = 1},
                            new PermisoRol {TipoPermiso = 3, RolId = 1, Estado = 1},
                            new PermisoRol {TipoPermiso = 4, RolId = 1, Estado = 1}
                        },
                        ItemTablaFormularioList = new List<ItemTablaFormulario>
                        {
                            new ItemTablaFormulario {ItemTablaId = 1, Estado = 1, Nombre = "Usuario"}
                        }
                    }
                }
            };

            var grupoOperacionOperaciones = new Formulario
            {
                Direccion = string.Empty,
                Controlador = string.Empty,
                Orden = 0,
                Nivel = 1,
                FormularioParent = moduloMantenimientoTest,
                Estado = 1,
                PermisoRolList = new List<PermisoRol>
                {
                    new PermisoRol {TipoPermiso = 1, RolId = 1, Estado = 1}
                },
                ItemTablaFormularioList = new List<ItemTablaFormulario>
                {
                    new ItemTablaFormulario {ItemTablaId = 1, Estado = 1, Nombre = "Operaciones"}
                },
                FormulariosHijosList = new List<Formulario>
                {
                    new Formulario
                    {
                        Direccion = "Administracion/Usuario/Index",
                        Controlador = "Compra",
                        Orden = 1,
                        Nivel = 2,
                        Estado = 1,
                        PermisoRolList = new List<PermisoRol>
                        {
                            new PermisoRol {TipoPermiso = 1, RolId = 1, Estado = 1},
                            new PermisoRol {TipoPermiso = 2, RolId = 1, Estado = 1},
                            new PermisoRol {TipoPermiso = 3, RolId = 1, Estado = 1},
                            new PermisoRol {TipoPermiso = 4, RolId = 1, Estado = 1}
                        },
                        ItemTablaFormularioList = new List<ItemTablaFormulario>
                        {
                            new ItemTablaFormulario {ItemTablaId = 1, Estado = 1, Nombre = "Compra"}
                        }
                    }
                }
            };

            context.Set<Formulario>().Add(grupoOperacionMantenedores);
            context.Set<Formulario>().Add(grupoOperacionOperaciones);

            #endregion

            #endregion
        }
    }
}