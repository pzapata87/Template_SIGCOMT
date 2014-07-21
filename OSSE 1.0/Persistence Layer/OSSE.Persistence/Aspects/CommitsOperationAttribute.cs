using System;
using PostSharp.Aspects;

namespace OSSE.Persistence.Aspects
{
    [Serializable]
    public sealed class CommitsOperationAttribute : MethodInterceptionAspect
    {
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
            var dispatcher = new MessageDispatcher {RelaseContext = ReleaseContext};

            if (args.ReturnValue == null)
                dispatcher.HandleCommand(args.Proceed);
        }
    }
}