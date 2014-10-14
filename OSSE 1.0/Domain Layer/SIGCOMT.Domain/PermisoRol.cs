using SIGCOMT.Domain.Core;

namespace SIGCOMT.Domain
{
    public class PermisoRol : Entity<int>
    {
        public int FormularioId { get; set; }
        public int RolId { get; set; }
        public int TipoPermiso { get; set; }

        public virtual Formulario Formulario { get; set; }
        public virtual Rol Rol { get; set; }
    }
}