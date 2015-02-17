using System;
using System.Reflection;
using log4net;
using PostSharp.Aspects;

namespace SIGCOMT.Aspects
{
    [Serializable]
    public sealed class TryCatchAttribute : OnMethodBoundaryAspect
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public TryCatchAttribute()
        {
            AspectPriority = 9;
            AttributePriority = 9;
        }

        public bool RethrowException { get; set; }
        public Type ExceptionTypeExpected { get; set; }

        public override void OnException(MethodExecutionArgs args)
        {
            if (args.Exception.GetType() == ExceptionTypeExpected)
            {
                Log.Error(args.Exception.Message, args.Exception);
                args.FlowBehavior = RethrowException ? FlowBehavior.RethrowException : FlowBehavior.Continue;
            }
        }
    }
}