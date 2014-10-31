using System.Data.Entity.ModelConfiguration;
using SIGCOMT.Domain;

namespace SIGCOMT.Persistence.DatabaseMappings
{
    public class PermisoFormularioRolConfiguration : EntityTypeConfiguration<PermisoFormularioRol>
    {
        public PermisoFormularioRolConfiguration()
        {
            HasRequired(p => p.PermisoFormulario).WithMany().HasForeignKey(p => new {p.FormularioId, p.TipoPermiso});
            HasRequired(p => p.Rol).WithMany(p => p.PermisoRolList).HasForeignKey(p => p.RolId);
        }
    }
}