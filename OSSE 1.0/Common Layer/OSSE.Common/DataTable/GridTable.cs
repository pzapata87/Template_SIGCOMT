using System.Collections.Generic;

namespace OSSE.Common.DataTable
{
    public class GridTable
    {
        public int draw { get; set; }
        public List<ColumnModel> columns { get; set; }
        public List<OrderColumn> order { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public SearchColumn search { get; set; }
    }
}