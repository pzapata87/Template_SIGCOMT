using System.Data.Entity;
using OSSE.Domain;
using OSSE.Persistence.Core;

namespace OSSE.Repository.SqlServer
{
    public class PermisoRolRepository : RepositoryWithTypedId<PermisoRol, int>, IPermisoRolRepository
    {
        public PermisoRolRepository(DbContext instanceDbContext)
            : base(instanceDbContext)
        {
        }
    }
}