using System.Collections.Generic;

namespace OSSE.Repository.RepositoryContracts
{
    public interface IRepositoryQueryExecutor
    {
        IEnumerable<TQ> SqlCommand<TQ>(string sql, params object[] parameters);
    }
}
