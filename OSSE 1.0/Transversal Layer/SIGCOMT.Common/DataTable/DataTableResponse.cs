using System.Collections.Generic;

namespace SIGCOMT.Common.DataTable
{
    public class DataTableResponse<T> where T : class
    {
        public List<T> data { get; set; }
        public int draw { get; set; }
        public int recordsFiltered { get; set; }
        public int recordsTotal { get; set; }
    }
}