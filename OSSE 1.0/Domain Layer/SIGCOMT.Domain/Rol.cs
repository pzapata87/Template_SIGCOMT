using System.Collections.Generic;
using SIGCOMT.Domain.Core;

namespace SIGCOMT.Domain
{
    public class Rol : Entity<int>
    {
        public string Nombre { get; set; }

        public virtual ICollection<RolUsuario> RolUsuarioList { get; set; }
        public virtual ICollection<PermisoRol> PermisoRolList { get; set; }
    }
}