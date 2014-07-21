using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using OSSE.Domain;

namespace OSSE.Persistence.DatabaseMappings
{
    public class TablaConfiguration : EntityTypeConfiguration<Tabla>
    {
        public TablaConfiguration()
        {
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(p => p.Nombre).IsRequired().HasMaxLength(40);
            Property(p => p.Descripcion).HasMaxLength(250);
        }
    }
}
