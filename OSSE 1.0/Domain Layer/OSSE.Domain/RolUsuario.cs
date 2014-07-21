using OSSE.Domain.Core;

namespace OSSE.Domain
{
    public class RolUsuario : EntityBase
    {
        public int UsuarioId { get; set; }
        public int RolId { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Rol Rol { get; set; }
    }
}
