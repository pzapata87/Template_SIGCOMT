using SIGCOMT.Domain;
using SIGCOMT.Domain.Core;
using SIGCOMT.DomainValidation.Core;
using SIGCOMT.DomainValidation.Validations;

namespace SIGCOMT.DomainValidation
{
    public static class ValidationFactory
    {
        static ValidationFactory()
        {
            ServiceLocator.Register<Usuario, UsuarioValidation>();
        }

        public static bool Validate(EntityBase entity)
        {
            var validationClass = ServiceLocator.Resolve(entity);
            return validationClass == null || validationClass.IsValid(entity);
        }
    }
}