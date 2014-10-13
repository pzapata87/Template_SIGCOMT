using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using OSSE.Common.Constantes;
using OSSE.Domain;

namespace OSSE.Web.Core.FilterAttributes
{
    public class ActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var formulariosEnSession = (List<Formulario>) HttpContext.Current.Session[MasterConstantes.Formularios];

            if (formulariosEnSession != null)
            {
                int id =
                    formulariosEnSession.Find(m => m.Controlador == filterContext.ActionDescriptor.ControllerDescriptor.ControllerName).Id;
                HttpContext.Current.Session.Add(MasterConstantes.IdControlador, id);
            }
            else
            {
                HttpContext.Current.Session.Add(MasterConstantes.IdControlador, 0);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}