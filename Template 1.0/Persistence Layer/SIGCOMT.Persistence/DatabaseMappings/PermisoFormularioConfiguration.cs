using System.Data.Entity.ModelConfiguration;
using SIGCOMT.Domain;

namespace SIGCOMT.Persistence.DatabaseMappings
{
    public class PermisoFormularioConfiguration : EntityTypeConfiguration<PermisoFormulario>
    {
        public PermisoFormularioConfiguration()
        {
            HasKey(p => new {p.FormularioId, p.TipoPermiso});
            HasRequired(p => p.Formulario).WithMany(p => p.PermisoList).HasForeignKey(p => p.FormularioId);
        }
    }
}