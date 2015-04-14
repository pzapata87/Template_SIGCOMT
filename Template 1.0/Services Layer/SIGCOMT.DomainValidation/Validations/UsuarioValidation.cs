using System.Linq;
using SIGCOMT.Domain;
using SIGCOMT.Domain.Core;
using SIGCOMT.DomainValidation.Core;
using SIGCOMT.DomainValidation.Excepciones;
using SIGCOMT.Repository;
using StructureMap;

namespace SIGCOMT.DomainValidation.Validations
{
    public class UsuarioValidation : IValidation
    {
        public bool IsValid(EntityBase entity)
        {
            var usuarioEntity = (Usuario)entity;
            var usuarioRepo = ObjectFactory.GetInstance<IUsuarioRepository>();

            var existe =
                usuarioRepo.FindAll(p => p.UserName == usuarioEntity.UserName && p.Id != usuarioEntity.Id).Any();

            if (existe)
                throw new UsuarioDuplicadoException();

            return true;
        }
    }
}