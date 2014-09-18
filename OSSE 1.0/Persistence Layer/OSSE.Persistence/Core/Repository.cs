using System.Data.Entity;
using OSSE.Repository.RepositoryContracts;

namespace OSSE.Persistence.Core
{
    public class Repository<T> : RepositoryWithTypedId<T, int>, IRepository<T> where T : class
    {
        public Repository(DbContext instanceDbContext) : base(instanceDbContext)
        {

        }
    }
}