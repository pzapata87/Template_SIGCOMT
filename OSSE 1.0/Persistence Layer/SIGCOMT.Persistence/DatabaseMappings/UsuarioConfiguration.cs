using System.Data.Entity.ModelConfiguration;
using SIGCOMT.Domain;

namespace SIGCOMT.Persistence.DatabaseMappings
{
    public class UsuarioConfiguration : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfiguration()
        {
            Property(p => p.Password).IsRequired().HasMaxLength(100);
            Property(p => p.Nombre).HasMaxLength(30);
            Property(p => p.UserName).IsRequired().HasMaxLength(100);
            Property(p => p.Apellido).HasMaxLength(50);
            Property(p => p.Email).IsRequired().HasMaxLength(50);
            Property(p => p.Telefono).HasMaxLength(50);
        }
    }
}