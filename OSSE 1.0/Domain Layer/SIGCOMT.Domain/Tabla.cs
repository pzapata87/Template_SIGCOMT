using System.Collections.Generic;
using SIGCOMT.Domain.Core;

namespace SIGCOMT.Domain
{
    public class Tabla : Entity<int>
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<ItemTabla> ItemTabla { get; set; }
    }
}