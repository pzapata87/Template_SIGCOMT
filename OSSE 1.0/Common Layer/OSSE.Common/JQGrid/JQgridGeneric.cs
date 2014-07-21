
namespace OSSE.Common.JQGrid
{
    public class JQgridGeneric<T>
    {
        public int Total { get; set; }

        public int Page { get; set; }

        public int Records { get; set; }

        public int Start { get; set; }

        public RowGeneric<T>[] Rows { get; set; }
    }
}