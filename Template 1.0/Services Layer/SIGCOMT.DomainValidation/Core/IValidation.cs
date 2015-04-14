using SIGCOMT.Domain.Core;

namespace SIGCOMT.DomainValidation.Core
{
    public interface IValidation
    {
        bool IsValid(EntityBase entity);
    }
}