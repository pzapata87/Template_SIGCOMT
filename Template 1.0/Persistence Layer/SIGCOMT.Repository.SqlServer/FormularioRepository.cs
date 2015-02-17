using System.Data.Entity;
using SIGCOMT.Domain;
using SIGCOMT.Persistence.Core;

namespace SIGCOMT.Repository.SqlServer
{
    public class FormularioRepository : RepositoryWithTypedId<Formulario, int>, IFormularioRepository
    {
        public FormularioRepository(DbContext instanceDbContext)
            : base(instanceDbContext)
        {
        }

        public override object[] GetKey(Formulario entity)
        {
            return new object[]
            {
                entity.Id
            };
        }
    }
}