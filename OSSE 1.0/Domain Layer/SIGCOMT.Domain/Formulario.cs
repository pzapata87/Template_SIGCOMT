using System.Collections.Generic;
using SIGCOMT.Domain.Core;

namespace SIGCOMT.Domain
{
    public class Formulario : Entity<int>
    {
        public string ResourceKey { get; set; }
        public string Direccion { get; set; }
        public int Orden { get; set; }
        public int? FormularioParentId { get; set; }

        public virtual Formulario FormularioParent { get; set; }

        public virtual ICollection<PermisoFormulario> PermisoList { get; set; }
        public virtual ICollection<Formulario> FormulariosHijosList { get; set; }
        public virtual ICollection<Formulario> ListaFormularios { get; set; }
    }
}