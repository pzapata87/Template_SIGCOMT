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
                Estado = usuarioDomain.Estado
            };
        }
    }
}