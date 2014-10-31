using System.Data.Entity.ModelConfiguration;
using SIGCOMT.Domain;

namespace SIGCOMT.Persistence.DatabaseMappings
{
    public class PermisoFormularioUsuarioConfiguration : EntityTypeConfiguration<PermisoFormularioUsuario>
    {
        public PermisoFormularioUsuarioConfiguration()
        {
            HasRequired(p => p.PermisoFormulario).WithMany().HasForeignKey(p => new {p.FormularioId, p.TipoPermiso});
            HasRequired(p => p.Usuario).WithMany(p => p.PermisoUsuarioList).HasForeignKey(p => p.UsuarioId);
        }
    }
}