using System.Collections.Generic;
using SIGCOMT.DTO;

namespace SIGCOMT.Cache
{
    public static class GlobalParameters
    {
        public static Dictionary<int, string> Idiomas { get; set; }
        public static Dictionary<int, List<PermisoFormularioDto>> PermisoFormularioList { get; set; }
    }
}
