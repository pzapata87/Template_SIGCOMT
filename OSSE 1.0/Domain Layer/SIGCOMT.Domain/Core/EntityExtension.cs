using System;

namespace OSSE.Domain.Core
{
    public class EntityExtension<TId> : Entity<TId>
    {
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}