using System.Linq;

namespace OSSE.Repository.Specifications
{
    public interface IQuerySpecification<T>
    {
        IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates);
    }
}