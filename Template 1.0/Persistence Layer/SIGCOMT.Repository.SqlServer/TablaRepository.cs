using System.Data.Entity;
using SIGCOMT.Domain;
using SIGCOMT.Persistence.Core;

namespace SIGCOMT.Repository.SqlServer
{
    public class TablaRepository : RepositoryWithTypedId<Tabla, int>, ITablaRepository
    {
        public TablaRepository(DbContext instanceDbContext)
            : base(instanceDbContext)
        {
        }
    }
}