using System;
using System.Collections.Generic;
using System.Linq;
using PostSharp.Aspects;
using SIGCOMT.Domain.Core;
using SIGCOMT.DomainValidation;
using StructureMap;

namespace SIGCOMT.Persistence.Aspects
{
    [Serializable]
    public sealed class CommitsOperationAttribute : MethodInterceptionAspect
    {
        private readonly IMessageDispatcher _dispatcher;

        public CommitsOperationAttribute()
        {
            AspectPriority = 10;
            AttributePriority = 10;
            ReleaseContext = true;
        }

        public bool SaveLogGeneral { get; set; }
        public int TablaId { get; set; }
        public int TipoAccionId { get; set; }
        public int UsuarioId { get; set; }

        public bool ReleaseContext { get; set; }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var dispatcher = ObjectFactory.GetInstance<IMessageDispatcher>();
            var argumentos = args.Arguments;

            if (ValidateArgumentos(argumentos))
            {
                dispatcher.RelaseContext = ReleaseContext;

                if (args.ReturnValue == null)
                    dispatcher.HandleCommand(args.Proceed);
            }
        }

        private bool ValidateArgumentos(IEnumerable<object> arguments)
        {
            var enumerable = arguments as IList<object> ?? arguments.ToList();
            var listasEntitiesBase = enumerable.OfType<IEnumerable<EntityBase>>().ToList();

            if (listasEntitiesBase.Any())
            {
                if (
                    listasEntitiesBase.Any(
                        listaEntitiesBase =>
                            listaEntitiesBase.Any(entityBase => !ValidationFactory.Validate(entityBase))))
                {
                    return false;
                }
            }

            return enumerable.OfType<EntityBase>().All(ValidationFactory.Validate);
        }
    }
}