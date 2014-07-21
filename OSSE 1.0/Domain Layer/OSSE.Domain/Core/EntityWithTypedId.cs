namespace OSSE.Domain.Core
{
    public class EntityWithTypedId<TId> : EntityBase
    {
        public TId Id { get; set; }
    }
}