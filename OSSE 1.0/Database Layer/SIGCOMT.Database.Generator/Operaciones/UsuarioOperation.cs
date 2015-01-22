using System.Collections.Generic;
using SIGCOMT.Common.Enum;
using SIGCOMT.DataBase.Generator.Core;
using SIGCOMT.Domain;

namespace SIGCOMT.DataBase.Generator.Operaciones
{
    public class UsuarioOperation : OperationBase
    {
        public UsuarioOperation(List<Rol> rolesConPermiso)
        {
            RolesConPermiso = rolesConPermiso;
            ResourceKey = TipoOperacion.UsuarioOperation.ToString();
            Direccion = UrlOperationManager.OperationsUrl[TipoOperacion.UsuarioOperation];

            PermisosOperacion = new List<TipoPermiso>
            {
                TipoPermiso.Mostrar,
                TipoPermiso.Crear,
                TipoPermiso.Editar,
                TipoPermiso.Eliminar
            };
        }
    }
}