using OSSE.DTO.Core;

namespace OSSE.DTO
{
    public class UsuarioDto : EntityDto<int>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Estado { get; set; }
    }
}
