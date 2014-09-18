using System.Data.Entity;
using OSSE.Domain;
using OSSE.Persistence.Core;

namespace OSSE.Repository.SqlServer
{
    public class TablaRepository : RepositoryWithTypedId<Tabla, int>, ITablaRepository 
    {
        public TablaRepository(DbContext instanceDbContext)
            : base(instanceDbContext)
        {
            
        }
    }
}
