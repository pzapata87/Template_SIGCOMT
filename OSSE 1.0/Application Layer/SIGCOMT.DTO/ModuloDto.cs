using System.Collections.Generic;
using SIGCOMT.DTO.Core;

namespace SIGCOMT.DTO
{
    public class ModuloDto : EntityDto<int>
    {
        public string Nombre { get; set; }
        public string Icono { get; set; }

        public List<OperacionDto> Operaciones { get; set; }
    }
}