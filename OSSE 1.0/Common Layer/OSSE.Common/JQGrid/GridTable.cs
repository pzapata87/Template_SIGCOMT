using System.Collections.Generic;

namespace OSSE.Common.JQGrid
{
    public class GridTable
    {
        public int Page { get; set; }

        public int Limit { get; set; }

        public string Sidx { get; set; }

        public string Sord { get; set; }

        public string Filters { get; set; }

        public List<Rule> Rules { get; set; }
    }
}