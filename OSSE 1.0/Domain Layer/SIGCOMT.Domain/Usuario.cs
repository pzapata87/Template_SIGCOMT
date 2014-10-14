using System.Collections.Generic;
using SIGCOMT.Domain.Core;

namespace SIGCOMT.Domain
{
    public class Usuario : Entity<int>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public int IdiomaId { get; set; }

        public virtual ICollection<RolUsuario> RolUsuarioList { get; set; }
    }
}