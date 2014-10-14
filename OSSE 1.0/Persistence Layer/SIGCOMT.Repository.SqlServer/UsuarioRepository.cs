using System.Data.Entity;
using SIGCOMT.Domain;
using SIGCOMT.Persistence.Core;

namespace SIGCOMT.Repository.SqlServer
{
    public class UsuarioRepository : RepositoryWithTypedId<Usuario, int>, IUsuarioRepository
    {
        public UsuarioRepository(DbContext instanceDbContext)
            : base(instanceDbContext)
        {
        }
    }
}