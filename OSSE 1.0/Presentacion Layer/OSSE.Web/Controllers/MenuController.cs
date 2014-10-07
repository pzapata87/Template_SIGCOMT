using System.Web.Mvc;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Web.Core;

namespace OSSE.Web.Controllers
{
    public class MenuController : BaseController
    {
        public MenuController(IFormularioBL formularioBL, IPermisoRolBL permisoRolBL, IItemTablaBL itemTablaBL)
            :base(formularioBL, permisoRolBL, itemTablaBL)
        {
            
        }

        // GET: Menu
        public ActionResult Index()
        {
            var formularios = ObtenerFormulariosUsuario();
            return PartialView(formularios);
        }
    }
}