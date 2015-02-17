using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using PostSharp.Aspects;

namespace SIGCOMT.Web.Core.Aspects
{
    [Serializable]
    public class ControllerAttribute : MethodInterceptionAspect
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Dictionary<TipoAccionControlador, IControllerMethod> _accionesMethods;

        public ControllerAttribute()
        {
            AspectPriority = 9;
            AttributePriority = 9;

            _accionesMethods = new Dictionary<TipoAccionControlador, IControllerMethod>
            {
                {TipoAccionControlador.Get, new GetControllerMethod()},
                {TipoAccionControlador.Post, new PostControllerMethod()}
            };
        }

        public TipoAccionControlador TipoVerbo { get; set; }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            _accionesMethods[TipoVerbo].Procesar(args, Log);
        }
    }
}