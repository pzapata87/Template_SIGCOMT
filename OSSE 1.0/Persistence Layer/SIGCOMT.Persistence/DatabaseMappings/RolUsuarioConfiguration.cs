using System.Data.Entity.ModelConfiguration;
using SIGCOMT.Domain;

namespace SIGCOMT.Persistence.DatabaseMappings
{
    public class RolUsuarioConfiguration : EntityTypeConfiguration<RolUsuario>
    {
        public RolUsuarioConfiguration()
        {
            HasKey(p => new {p.UsuarioId, p.RolId});
            HasRequired(p => p.Usuario).WithMany(p => p.RolUsuarioList).HasForeignKey(p => p.UsuarioId).WillCascadeOnDelete(false);
            HasRequired(p => p.Rol).WithMany(p => p.RolUsuarioList).HasForeignKey(p => p.RolId).WillCascadeOnDelete(false);
        }
    }
}