using System.Data.Entity;
using SIGCOMT.Domain;
using SIGCOMT.Persistence.Core;

namespace SIGCOMT.Repository.SqlServer
{
    public class FormularioRepository : RepositoryWithTypedId<Formulario, int>, IFormularioRepository
    {
        //public List<Formulario> Formularios(int idRol)
        //{
        //    return Set.Where(p => p.PermisoRolList.Any(q => q.RolId == idRol && q.TipoPermiso == (int)TipoPermiso.Mostrar) && 
        //        p.ModuloId == idModulo && 
        //        p.Estado == (int)TipoEstado.Activo)
        //        .Distinct().OrderBy(r => r.Orden).ToList();
        //}

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