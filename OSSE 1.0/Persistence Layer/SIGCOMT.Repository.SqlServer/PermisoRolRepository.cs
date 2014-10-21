using System.Data.Entity;
using SIGCOMT.Domain;
using SIGCOMT.Persistence.Core;

namespace SIGCOMT.Repository.SqlServer
{
    public class PermisoRolRepository : RepositoryWithTypedId<PermisoFormularioRol, int>, IPermisoRolRepository
    {
        public PermisoRolRepository(DbContext instanceDbContext)
            : base(instanceDbContext)
        {
        }
    }
}