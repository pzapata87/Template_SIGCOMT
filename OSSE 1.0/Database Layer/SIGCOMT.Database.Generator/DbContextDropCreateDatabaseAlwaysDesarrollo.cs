using System.Collections.Generic;
using System.Data.Entity;
using SIGCOMT.Common;
using SIGCOMT.Common.Enum;
using SIGCOMT.DataBase.Generator.Core;
using SIGCOMT.DataBase.Generator.Modulos;
using SIGCOMT.Domain;
using SIGCOMT.Persistence.EntityFramework;

namespace SIGCOMT.DataBase.Generator
{
    public class DbContextDropCreateDatabaseAlwaysDesarrollo : DropCreateDatabaseAlways<DbContextBase>
    {
        private ItemTabla _idiomaDefaul;
        private Rol _rolAdmin;

        protected override void Seed(DbContextBase context)
        {
            AgregarRegistrosTabla(context);
            var roles = AgregarRegistrosRol(context);
            _rolAdmin = roles[0];

            AgregarRegistrosUsuario(context);
            AgregarRegistrosModulo(context);
        }

        private void AgregarRegistrosTabla(DbContext context)
        {
            _idiomaDefaul = new ItemTabla
            {
                Nombre = "es-PE",
                Descripcion = "Español",
                Estado = TipoEstado.Activo.GetNumberValue(),
                Valor = "1"
            };

            var idiomas = new List<ItemTabla> { _idiomaDefaul };
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
                            Nombre = "Editar",
                            Descripcion = string.Empty,
                            Estado = activo,
                            Valor = TipoPermiso.Editar.GetStringValue()
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
        }

        private List<Rol> AgregarRegistrosRol(DbContext context)
        {
            var roles = new List<Rol>
            {
                new Rol {Nombre = TipoRol.Administrador.ToString(), Estado = TipoEstado.Activo.GetNumberValue()}
            };

            context.Set<Rol>().AddRange(roles);

            RolesStorage.Roles.Add(TipoRol.Administrador, roles[0]);

            return roles;
        }

        private void AgregarRegistrosUsuario(DbContext context)
        {
            var usuario = new Usuario
            {
                UserName = "Admin",
                Password = Encriptador.Encriptar("1234"),
                Email = "admin@sigcomt.com",
                Estado = TipoEstado.Activo.GetNumberValue(),
                IdiomaId = int.Parse(_idiomaDefaul.Valor),
                RolUsuarioList = new[]
                {
                    new RolUsuario {Rol = _rolAdmin, Estado = TipoEstado.Activo.GetNumberValue()}
                }
            };

            context.Set<Usuario>().Add(usuario);
        }

        private void AgregarRegistrosModulo(DbContext context)
        {
            var modulos = new List<IModulo>
            {
                new SeguridadModulo()
            };

            foreach (var modulo in modulos)
            {
                modulo.Registrar(context);
            }
        }
    }
}