using System.Collections.Generic;

namespace OSSE.Common.JQGrid
{
    public class GridTable
    {
        public int Page { get; set; }

        public int Rows { get; set; }

        public string Sidx { get; set; }

        public string Sord { get; set; }

        public bool Search { get; set; }

        public string SearchField { get; set; }

        public string SearchOper { get; set; }

        public string SearchString { get; set; }

        public string Filters { get; set; }

        public List<Rule> Rules { get; set; }
    }
}