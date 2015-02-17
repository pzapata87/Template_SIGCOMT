using System.Linq;

namespace SIGCOMT.Repository.Specifications
{
    public interface IQuerySpecification<T>
    {
        IQueryable<T> SatisfyingElementsFrom(IQueryable<T> candidates);
    }
}