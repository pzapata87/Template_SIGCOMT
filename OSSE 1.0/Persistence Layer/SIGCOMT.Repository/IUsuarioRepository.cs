using OSSE.Domain;
using OSSE.Repository.RepositoryContracts;

namespace OSSE.Repository
{
    public interface IUsuarioRepository : IRepositoryWithTypedId<Usuario, int>
    {
    }
}