using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using log4net;
using PostSharp.Aspects;
using SIGCOMT.Common;
using SIGCOMT.Resources;

namespace SIGCOMT.Web.Core.Aspects
{
    [Serializable]
    public class PostControllerMethod : IControllerMethod
    {
        public void Procesar(MethodInterceptionArgs args, ILog log)
        {
            var controller = (BaseController)args.Instance;

            if (controller.ModelState.IsValid)
            {
                try
                {
                    args.Proceed();
                }
                catch (InvalidDataException ex)
                {
                    log.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));

                    var jsonResponse = new JsonResponse { Success = false, Message = ex.Message };
                    args.ReturnValue = controller.Json(jsonResponse, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    log.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));

                    var jsonResponse = new JsonResponse { Success = false, Message = Master.MensajeReintentar };
                    args.ReturnValue = controller.Json(jsonResponse, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string message = string.Join("<br>",
                    controller.ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage));

                var jsonResponse = new JsonResponse { Success = false, Message = message };
                args.ReturnValue = controller.Json(jsonResponse, JsonRequestBehavior.AllowGet);
            }
        }
    }
}