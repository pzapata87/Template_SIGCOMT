using SIGCOMT.Domain;
using SIGCOMT.Repository.RepositoryContracts;

namespace SIGCOMT.Repository
{
    public interface IUsuarioRepository : IRepositoryWithTypedId<Usuario, int>
    {
    }
}