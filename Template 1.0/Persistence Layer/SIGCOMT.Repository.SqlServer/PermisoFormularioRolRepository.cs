using System.Data.Entity;
using SIGCOMT.Domain;
using SIGCOMT.Persistence.Core;

namespace SIGCOMT.Repository.SqlServer
{
    public class PermisoFormularioRolRepository : RepositoryWithTypedId<PermisoFormularioRol, int>, IPermisoFormularioRolRepository
    {
        public PermisoFormularioRolRepository(DbContext instanceDbContext)
            : base(instanceDbContext)
        {
        }
    }
}