using SIGCOMT.DTO.Core;

namespace SIGCOMT.DTO
{
    public class UsuarioDto : EntityDto<int>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Idioma { get; set; }
        public int Estado { get; set; }
    }
}