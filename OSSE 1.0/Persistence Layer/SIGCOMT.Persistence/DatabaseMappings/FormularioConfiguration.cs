using System.Data.Entity.ModelConfiguration;
using OSSE.Domain;

namespace OSSE.Persistence.DatabaseMappings
{
    public class FormularioConfiguration : EntityTypeConfiguration<Formulario>
    {
        public FormularioConfiguration()
        {
            Property(p => p.Direccion).HasMaxLength(4000);
            Property(p => p.Controlador).HasMaxLength(100);

            HasOptional(p => p.FormularioParent).WithMany(p => p.FormulariosHijosList).HasForeignKey(p => p.FormularioParentId);
            Ignore(p => p.ListaFormularios);
        }
    }
}