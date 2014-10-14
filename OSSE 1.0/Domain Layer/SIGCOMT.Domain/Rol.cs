using System.Collections.Generic;
using OSSE.Domain.Core;

namespace OSSE.Domain
{
    public class Rol : Entity<int>
    {
        public string Nombre { get; set; }

        public virtual ICollection<RolUsuario> RolUsuarioList { get; set; }
        public virtual ICollection<PermisoRol> PermisoRolList { get; set; }
    }
}