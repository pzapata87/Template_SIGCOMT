using System.Collections.Generic;
using SIGCOMT.Common.Constantes;
using SIGCOMT.Common.Enum;
using SIGCOMT.DataBase.Generator.Core;
using SIGCOMT.DataBase.Generator.Operaciones;
using SIGCOMT.Domain;

namespace SIGCOMT.DataBase.Generator.Modulos
{
    public class SeguridadModulo : ModuloBase
    {
        public SeguridadModulo()
        {
            RolesConPermiso = new List<Rol> { RolesStorage.Roles[TipoRol.Administrador] };
            ResourceKey = TipoModulo.SeguridadModulo.ToString();
            IconoModulo = IconosSvgConstantes.ModuloSeguridad;

            Operations = new List<IOperation>
            {
                new UsuarioOperation(new List<Rol> {RolesStorage.Roles[TipoRol.Administrador]}),
                new FormularioOperation(new List<Rol> {RolesStorage.Roles[TipoRol.Administrador]})
            };
        }
    }
}