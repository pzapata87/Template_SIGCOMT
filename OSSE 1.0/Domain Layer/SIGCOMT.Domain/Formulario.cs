using System.Collections.Generic;
using OSSE.Domain.Core;

namespace OSSE.Domain
{
    public class Formulario : Entity<int>
    {
        public string Direccion { get; set; }
        public int Orden { get; set; }
        public int Nivel { get; set; }
        public int? FormularioParentId { get; set; }
        public string Controlador { get; set; }

        public virtual Formulario FormularioParent { get; set; }

        public virtual ICollection<PermisoRol> PermisoRolList { get; set; }
        public virtual ICollection<Formulario> FormulariosHijosList { get; set; }
        public virtual ICollection<ItemTablaFormulario> ItemTablaFormularioList { get; set; }

        public virtual ICollection<Formulario> ListaFormularios { get; set; }
    }
}