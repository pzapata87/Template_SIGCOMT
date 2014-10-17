using System.Collections.Generic;

namespace SIGCOMT.Common.DataTable
{
    public class ColumnInformation
    {
        public string Columna { get; set; }
        public string Operador { get; set; }
        public List<ValorHomologacion> Valores { get; set; }
    }
}