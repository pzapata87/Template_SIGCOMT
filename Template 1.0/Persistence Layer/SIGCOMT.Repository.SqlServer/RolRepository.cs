using System.Data.Entity;
using SIGCOMT.Domain;
using SIGCOMT.Persistence.Core;

namespace SIGCOMT.Repository.SqlServer
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