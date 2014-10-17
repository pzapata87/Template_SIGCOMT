using System;
using log4net;
using PostSharp.Aspects;

namespace SIGCOMT.Web.Core.Aspects
{
    [Serializable]
    public class GetControllerMethod : IControllerMethod
    {
        public void Procesar(MethodInterceptionArgs args, ILog log)
        {
            try
            {
                args.Proceed();
            }
            catch (Exception ex)
            {
                var controller = (BaseController)args.Instance;

                log.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                args.ReturnValue = controller.RedirectToAction("Index", "Error", new { area = "" });
            }
        }
    }
}