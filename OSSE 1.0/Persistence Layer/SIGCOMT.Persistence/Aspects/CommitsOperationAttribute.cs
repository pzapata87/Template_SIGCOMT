using System;
using PostSharp.Aspects;
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
            dispatcher.RelaseContext = ReleaseContext;

            if (args.ReturnValue == null)
                dispatcher.HandleCommand(args.Proceed);
        }
    }
}