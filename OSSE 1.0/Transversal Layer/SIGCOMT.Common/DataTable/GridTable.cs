using System.Collections.Generic;

namespace SIGCOMT.Common.DataTable
{
    public class GridTable
    {
        public int Draw { get; set; }
        public List<ColumnModel> Columns { get; set; }
        public List<OrderColumn> Order { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public List<ColumnInformation> Homologaciones { get; set; }
        public SearchColumn Search { get; set; }
    }
}