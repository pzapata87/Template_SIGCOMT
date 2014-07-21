using OSSE.Domain;
using OSSE.Persistence.Core;

namespace OSSE.Repository.SqlServer
{
    public class UsuarioRepository : RepositoryWithTypedId<Usuario, int>, IUsuarioRepository
    {
    }
}