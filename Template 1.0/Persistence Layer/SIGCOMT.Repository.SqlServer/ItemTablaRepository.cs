using System.Data.Entity;
using SIGCOMT.Domain;
using SIGCOMT.Persistence.Core;

namespace SIGCOMT.Repository.SqlServer
{
    public class ItemTablaRepository : RepositoryWithTypedId<ItemTabla, int>, IItemTablaRepository
    {
        public ItemTablaRepository(DbContext instanceDbContext)
            : base(instanceDbContext)
        {
        }

        public override object[] GetKey(ItemTabla entity)
        {
            return new object[]
            {
                entity.Id
            };
        }
    }
}