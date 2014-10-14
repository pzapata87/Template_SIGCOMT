using System.Web.Mvc;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Web.Core;

namespace OSSE.Web.Controllers
{
    public class ErrorController : BaseController
    {
        public ErrorController(IFormularioBL formularioBL, IPermisoRolBL permisoRolBL, IItemTablaBL itemTablaBL) :
            base(formularioBL, permisoRolBL, itemTablaBL)
        {
        }

        //
        // GET: /Error/
        public ActionResult Index()
        {
            ViewBag.TituloBanner = "Error al procesar la solicitud";
            return View("Error");
        }
    }
}