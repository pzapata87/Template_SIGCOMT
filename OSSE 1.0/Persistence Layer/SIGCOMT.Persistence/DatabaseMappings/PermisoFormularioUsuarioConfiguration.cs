using System.Data.Entity.ModelConfiguration;
using SIGCOMT.Domain;

namespace SIGCOMT.Persistence.DatabaseMappings
{
    public class PermisoFormularioUsuarioConfiguration : EntityTypeConfiguration<PermisoFormularioUsuario>
    {
        public PermisoFormularioUsuarioConfiguration()
        {
            HasRequired(p => p.Formulario).WithMany().HasForeignKey(p => p.FormularioId);
            HasRequired(p => p.Usuario).WithMany(p => p.PermisoUsuarioList).HasForeignKey(p => p.UsuarioId);
        }
    }
}