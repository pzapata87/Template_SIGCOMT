using System;

namespace SIGCOMT.Domain.Core
{
    public class EntityExtension<TId> : Entity<TId>
    {
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}