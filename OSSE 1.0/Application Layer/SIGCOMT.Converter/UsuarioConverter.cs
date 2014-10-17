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
    }
}