using System.Collections.Generic;
using OSSE.Domain.Core;

namespace OSSE.Domain
{
    public class ItemTabla : Entity<int>
    {
        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual string Valor { get; set; }
        public virtual int TablaId { get; set; }

        public virtual Tabla Tabla { get; set; }
        public virtual ICollection<ItemTablaFormulario> ItemTablaFormulario { get; set; }
    }
}