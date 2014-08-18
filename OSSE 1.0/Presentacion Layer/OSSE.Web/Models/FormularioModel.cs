using System.Collections.Generic;
using OSSE.Common.Enum;
using OSSE.Domain;

namespace OSSE.Web.Models
{
    public class FormularioModel
    {
        public Formulario Formulario { get; set; }
        public bool Mostrar { get; set; }
        public bool Crear { get; set; }
        public bool Modificar { get; set; }
        public bool Eliminar { get; set; }
        public bool Imprimir { get; set; }
        public bool Mover { get; set; }
        public bool Reportar { get; set; }

        public FormularioModel(Formulario formulario, IEnumerable<PermisoRol> permisos)
        {
            Formulario = formulario;

            foreach (PermisoRol permiso in permisos)
            {
                if (permiso.TipoPermiso == (int)TipoPermiso.Mostrar) Mostrar = true;
                if (permiso.TipoPermiso == (int)TipoPermiso.Crear) Crear = true;
                if (permiso.TipoPermiso == (int)TipoPermiso.Modificar) Modificar = true;
                if (permiso.TipoPermiso == (int)TipoPermiso.Eliminar) Eliminar = true;                
                if (permiso.TipoPermiso == (int)TipoPermiso.Imprimir) Imprimir = true;
                if (permiso.TipoPermiso == (int)TipoPermiso.Mover) Mover = true;
                if (permiso.TipoPermiso == (int)TipoPermiso.Reportar) Reportar = true;
            }
        }
    }
}