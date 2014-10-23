using SIGCOMT.Domain.Core;

namespace SIGCOMT.Domain
{
    public class PermisoFormularioUsuario: Entity<int>
    {
        public int FormularioId { get; set; }
        public int UsuarioId { get; set; }
        public int TipoPermiso { get; set; }

        public Formulario Formulario { get; set; }
        public Usuario Usuario { get; set; }
    }
}
