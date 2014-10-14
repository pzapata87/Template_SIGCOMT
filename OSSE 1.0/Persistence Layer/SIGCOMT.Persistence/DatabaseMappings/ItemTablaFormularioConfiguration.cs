using System.Data.Entity.ModelConfiguration;
using SIGCOMT.Domain;

namespace SIGCOMT.Persistence.DatabaseMappings
{
    public class ItemTablaFormularioConfiguration : EntityTypeConfiguration<ItemTablaFormulario>
    {
        public ItemTablaFormularioConfiguration()
        {
            Property(p => p.Nombre).IsRequired().HasMaxLength(50);

            HasRequired(p => p.Formulario).WithMany(p => p.ItemTablaFormularioList).HasForeignKey(p => p.FormularioId);
            HasRequired(p => p.ItemTabla).WithMany(p => p.ItemTablaFormulario).HasForeignKey(p => p.ItemTablaId);
        }
    }
}