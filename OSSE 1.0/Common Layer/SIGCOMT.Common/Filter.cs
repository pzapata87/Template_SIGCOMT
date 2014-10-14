using System.Collections.Generic;

namespace SIGCOMT.Common
{
    public class Filter
    {
        public string GroupOp { get; set; }

        public List<Rule> Rules { get; set; }

        public List<Filter> Groups { get; set; }
    }
}