using System.Data.Entity.ModelConfiguration;
using SIGCOMT.Domain;

namespace SIGCOMT.Persistence.DatabaseMappings
{
    public class RolConfiguration : EntityTypeConfiguration<Rol>
    {
        public RolConfiguration()
        {
            Property(p => p.Nombre).IsRequired().HasMaxLength(100);
        }
    }
}