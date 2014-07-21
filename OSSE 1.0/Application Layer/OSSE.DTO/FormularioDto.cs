using System.Collections.Generic;
using OSSE.DTO.Core;

namespace OSSE.DTO
{
    public class FormularioDto : EntityDto<int>
    {
        public string Nombre { get; set; }
        public string Icono { get; set; }

        public List<OperacionDto> Operaciones { get; set; }
    }
}
