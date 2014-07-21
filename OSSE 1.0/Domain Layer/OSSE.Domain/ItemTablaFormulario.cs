using OSSE.Domain.Core;

namespace OSSE.Domain
{
    public class ItemTablaFormulario : Entity<int>
    {
        public virtual int FormularioId { get; set; }
        public virtual string Nombre { get; set; }
        public virtual int ItemTablaId { get; set; }

        public virtual Formulario Formulario { get; set; }
        public virtual ItemTabla ItemTabla { get; set; }
    }
}
