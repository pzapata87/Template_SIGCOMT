using System.Data.Entity;
using SIGCOMT.Repository.RepositoryContracts;

namespace SIGCOMT.Persistence.Core
{
    public class Repository<T> : RepositoryWithTypedId<T, int>, IRepository<T> where T : class
    {
        public Repository(DbContext instanceDbContext) : base(instanceDbContext)
        {
        }
    }
}