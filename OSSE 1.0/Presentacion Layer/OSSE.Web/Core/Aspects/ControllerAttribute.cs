using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using log4net;
using OSSE.Common;
using PostSharp.Aspects;
using Resources;

namespace OSSE.Web.Core.Aspects
{
    [Serializable]
    public class ControllerAttribute : MethodInterceptionAspect
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ControllerAttribute()
        {
            AspectPriority = 9;
            AttributePriority = 9;
        }

        public TipoAccionControlador TipoVerbo { get; set; }

        public override void OnInvoke(MethodInterceptionArgs args)
        {
            switch (TipoVerbo)
            {
                case TipoAccionControlador.Get:
                {
                    try
                    {
                        args.Proceed();
                    }
                    catch (Exception ex)
                    {
                        var controller = (BaseController) args.Instance;

                        Log.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                        args.ReturnValue = controller.RedirectToAction("Error", "Error");
                    }

                    break;
                }

                case TipoAccionControlador.Post:
                {
                    var controller = (BaseController) args.Instance;

                    if (controller.ModelState.IsValid)
                    {
                        try
                        {
                            args.Proceed();
                        }
                        catch (InvalidDataException ex)
                        {
                            Log.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));

                            var jsonResponse = new JsonResponse {Success = false, Message = ex.Message};
                            args.ReturnValue = controller.Json(jsonResponse, JsonRequestBehavior.AllowGet);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));

                            var jsonResponse = new JsonResponse {Success = false, Message = Master.MensajeReintentar};
                            args.ReturnValue = controller.Json(jsonResponse, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        string message = string.Join("; ",
                            controller.ModelState.Values.SelectMany(p => p.Errors).Select(p => p.ErrorMessage));
                        var jsonResponse = new JsonResponse {Success = false, Message = message};
                        args.ReturnValue = controller.Json(jsonResponse, JsonRequestBehavior.AllowGet);
                    }

                    break;
                }
                case TipoAccionControlador.Listado:
                {
                    var controller = (BaseController) args.Instance;

                    try
                    {
                        args.Proceed();
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));

                        controller.Response.StatusCode = 404;
                        args.ReturnValue = controller.Json(new JsonResponse {Message = "Ocurrio un error al cargar..."},
                            JsonRequestBehavior.AllowGet);
                    }

                    break;
                }
            }
        }
    }
}