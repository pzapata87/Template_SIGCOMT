using System.Collections.Generic;
using System.Data.Entity;
using SIGCOMT.Repository.RepositoryContracts;

namespace SIGCOMT.Persistence.Core
{
    public class RepositoryQueryExecutor : IRepositoryQueryExecutor
    {
        private readonly DbContext _instanceDB;

        public RepositoryQueryExecutor(DbContext instanceDbContext)
        {
            _instanceDB = instanceDbContext;
        }

        public IEnumerable<TQ> SqlCommand<TQ>(string sql, params object[] parameters)
        {
            return _instanceDB.Database.SqlQuery<TQ>(sql, parameters);
        }
    }
}