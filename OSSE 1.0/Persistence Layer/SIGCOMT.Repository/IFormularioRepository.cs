using OSSE.Domain;
using OSSE.Repository.RepositoryContracts;

namespace OSSE.Repository
{
    public interface IFormularioRepository : IRepositoryWithTypedId<Formulario, int>
    {
        //List<Formulario> Formularios(int idRol);
    }
}