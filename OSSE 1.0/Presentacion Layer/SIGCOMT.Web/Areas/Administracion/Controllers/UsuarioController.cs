using System.Web.Mvc;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Web.Core;

namespace OSSE.Web.Areas.Administracion.Controllers
{
    public class UsuarioController : BaseController
    {
        public UsuarioController(IFormularioBL formularioBL, IPermisoRolBL permisoRolBL, IItemTablaBL itemTablaBL)
             :base(formularioBL, permisoRolBL, itemTablaBL)
        {
            
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}