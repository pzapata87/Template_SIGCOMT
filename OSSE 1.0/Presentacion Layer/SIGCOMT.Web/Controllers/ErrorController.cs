using System.Web.Mvc;
using SIGCOMT.BusinessLogic.Interfaces;
using SIGCOMT.Web.Core;

namespace SIGCOMT.Web.Controllers
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