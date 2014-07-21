using System.Data.Entity.ModelConfiguration;
using OSSE.Domain;

namespace OSSE.Persistence.DatabaseMappings
{
    public class ItemTablaConfiguration : EntityTypeConfiguration<ItemTabla>
    {
        public ItemTablaConfiguration()
        {
            Property(p => p.Nombre).HasMaxLength(200);
            Property(p => p.Descripcion).HasMaxLength(500);
            Property(p => p.Valor).IsRequired().HasMaxLength(10);

            HasRequired(p => p.Tabla).WithMany(p => p.ItemTabla).HasForeignKey(p => p.TablaId);
        }
    }
}
