using System.Collections.Generic;
using System.Linq;
using SIGCOMT.Common;
using SIGCOMT.Domain;
using SIGCOMT.DTO;

namespace SIGCOMT.Converter
{
    public class UsuarioConverter
    {
        public static UsuarioDto DomainToDto(Usuario usuarioDomain)
        {
            return new UsuarioDto
            {
                Id = usuarioDomain.Id,
                UserName = usuarioDomain.UserName,
                Nombre = usuarioDomain.Nombre,
                Apellido = usuarioDomain.Apellido,
                Email = usuarioDomain.Email,
                Password = Encriptador.Desencriptar(usuarioDomain.Password),
                IdiomaId = usuarioDomain.IdiomaId,
                Telefono = usuarioDomain.Telefono,
                Estado = usuarioDomain.Estado
            };
        }

        public static void DtoToDomain(Usuario usuarioDomain, UsuarioDto usuarioDto)
        {
            usuarioDomain.Apellido = usuarioDto.Apellido;
            usuarioDomain.Nombre = usuarioDto.Nombre;
            usuarioDomain.Password = Encriptador.Encriptar(usuarioDto.Password);
            usuarioDomain.Telefono = usuarioDto.Telefono;
            usuarioDomain.Email = usuarioDto.Email;
            usuarioDomain.UserName = usuarioDto.UserName;
            usuarioDomain.IdiomaId = usuarioDto.IdiomaId;
        }

        public static List<PermisoFormularioDto> PermisosFormulario(IEnumerable<PermisoFormularioUsuario> permisos, IEnumerable<RolUsuario> roles)
        {
            var list = roles.SelectMany(p => p.Rol.PermisoRolList.Select(q =>
                new PermisoFormularioDto
                {
                    TipoPermiso = q.TipoPermiso,
                    FormularioId = q.FormularioId
                })).Union(permisos.Select(p => new PermisoFormularioDto
                {
                    TipoPermiso = p.FormularioId,
                    FormularioId = p.FormularioId
                }));

            return list.ToList();
        }
    }
}