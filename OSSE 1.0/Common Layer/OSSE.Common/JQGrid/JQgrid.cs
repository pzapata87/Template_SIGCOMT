using System.Collections.Generic;

namespace OSSE.Common.JQGrid
{
    public class JQgrid<T>
    {
        public int TotalPaginas { get; set; }

        public int Page { get; set; }

        public int CantidadRegistros { get; set; }

        public int Start { get; set; }

        public IEnumerable<T> Registros { get; set; }
    }
}