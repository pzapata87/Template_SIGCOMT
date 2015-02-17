using System.Collections.Generic;
using SIGCOMT.Domain.Core;

namespace SIGCOMT.Domain
{
    public class PermisoFormulario : EntityBase
    {
        public int FormularioId { get; set; }
        public int TipoPermiso { get; set; }

        public virtual Formulario Formulario { get; set; }
        public virtual ICollection<PermisoFormularioRol> PermisoFormularioRolList { get; set; }
    }
}