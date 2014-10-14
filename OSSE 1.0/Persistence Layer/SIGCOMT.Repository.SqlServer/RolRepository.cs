using System.Data.Entity;
using OSSE.Domain;
using OSSE.Persistence.Core;

namespace OSSE.Repository.SqlServer
{
    public class RolRepository : RepositoryWithTypedId<Rol, int>, IRolRepository
    {
        public RolRepository(DbContext instanceDbContext)
            : base(instanceDbContext)
        {
        }

        public override object[] GetKey(Rol entity)
        {
            return new object[]
            {
                entity.Id
            };
        }
    }
}