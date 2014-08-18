using System;
using System.Web.Mvc;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Web.Core;

namespace OSSE.Web.Controllers
{
    [Authorize]
    public class LoadController : BaseController
    {
        public LoadController(IFormularioBL formularioBL, IPermisoRolBL permisoRolBL, IItemTablaBL itemTablaBL) :
            base(formularioBL, permisoRolBL, itemTablaBL)
        {
        }


        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult NotAuthorized()
        {
            try
            {
                return PartialView("NotAuthorized");
            }
            catch (Exception ex)
            {
                LogError(ex);
                return RedirectToAction("NotAuthorized", "Error");
            }
        }
    }
}
