using System.Data.Entity.ModelConfiguration;
using OSSE.Domain;

namespace OSSE.Persistence.DatabaseMappings
{
    public class RolConfiguration : EntityTypeConfiguration<Rol>
    {
        public RolConfiguration()
        {
            Property(p => p.Nombre).IsRequired().HasMaxLength(100);
        }
    }
}
