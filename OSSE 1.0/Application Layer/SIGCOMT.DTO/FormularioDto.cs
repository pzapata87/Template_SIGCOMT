using System.Collections.Generic;

namespace SIGCOMT.DTO
{
    public class FormularioDto
    {
        public int Id { get; set; }
        public string Modulo { get; set; }
        public string Nombre { get; set; }
        public List<PermisoFormularioDto> PermisoList { get; set; }
    }
}