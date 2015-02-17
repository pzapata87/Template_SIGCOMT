using System.Collections.Generic;
using SIGCOMT.Common.Enum;
using SIGCOMT.DataBase.Generator.Core;
using SIGCOMT.Domain;

namespace SIGCOMT.DataBase.Generator.Operaciones
{
    public class FormularioOperation : OperationBase
    {
        public FormularioOperation(List<Rol> rolesConPermiso)
        {
            RolesConPermiso = rolesConPermiso;
            ResourceKey = TipoOperacion.FormularioOperation.ToString();
            Direccion = UrlOperationManager.OperationsUrl[TipoOperacion.FormularioOperation];

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