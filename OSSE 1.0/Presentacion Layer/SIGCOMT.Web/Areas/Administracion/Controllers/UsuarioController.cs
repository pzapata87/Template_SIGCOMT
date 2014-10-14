using System.Web.Mvc;
using SIGCOMT.BusinessLogic.Interfaces;
using SIGCOMT.Web.Core;

namespace SIGCOMT.Web.Areas.Administracion.Controllers
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