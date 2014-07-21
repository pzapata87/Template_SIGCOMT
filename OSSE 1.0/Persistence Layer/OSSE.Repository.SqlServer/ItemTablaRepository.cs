using OSSE.Domain;
using OSSE.Persistence.Core;

namespace OSSE.Repository.SqlServer
{
    public class ItemTablaRepository : RepositoryWithTypedId<ItemTabla, int>, IItemTablaRepository 
    {
        public override object[] GetKey(ItemTabla entity)
        {
            return new object[] 
            {
                entity.Id
            };
        }
    }
}
