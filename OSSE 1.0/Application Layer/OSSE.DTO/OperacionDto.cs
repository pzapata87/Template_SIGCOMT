using System.Collections.Generic;
using OSSE.DTO.Core;

namespace OSSE.DTO
{
    public class OperacionDto : EntityDto<int>
    {
        public string Nombre { get; set; }
        public string Controlador { get; set; }

        public List<OperacionDto> Operaciones { get; set; }
    }
}