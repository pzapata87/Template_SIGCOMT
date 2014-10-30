using System.Collections.Generic;
using System.Linq;
using SIGCOMT.Cache;
using SIGCOMT.Common;
using SIGCOMT.Domain;
using SIGCOMT.DTO;
using Usuario = SIGCOMT.Domain.Usuario;

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
                Password = Security.Desencriptar(usuarioDomain.Password),
                IdiomaId = usuarioDomain.IdiomaId,
                Telefono = usuarioDomain.Telefono,
                Estado = usuarioDomain.Estado
            };
        }

        public static void DtoToDomain(Usuario usuarioDomain, UsuarioDto usuarioDto)
        {
            usuarioDomain.Apellido = usuarioDto.Apellido;
            usuarioDomain.Nombre = usuarioDto.Nombre;
            usuarioDomain.Password = Security.Encriptar(usuarioDto.Password);
            usuarioDomain.Telefono = usuarioDto.Telefono;
            usuarioDomain.Email = usuarioDto.Email;
            usuarioDomain.UserName = usuarioDto.UserName;
            usuarioDomain.IdiomaId = usuarioDto.IdiomaId;
        }

        public static List<PermisoFormularioDto> PermisosFormulario(IEnumerable<PermisoFormularioUsuario> permisos, IEnumerable<RolUsuario> roles)
        {
            var list = roles.SelectMany(x => GlobalParameters.PermisoFormularioList.SelectMany(
                p => p.Value.Where(q => q.RolId == x.RolId).Select(r => new PermisoFormularioDto
                {
                    TipoPermiso = r.TipoPermiso,
                    Id = p.Key
                }))).Union(permisos.Select(p => new PermisoFormularioDto
                {
                    TipoPermiso = p.TipoPermiso,
                    Id = p.FormularioId
                }));

            return list.ToList();
        }
    }
}