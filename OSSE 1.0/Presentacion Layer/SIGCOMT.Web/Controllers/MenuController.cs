using System.Collections.Generic;
using System.Web.Mvc;
using SIGCOMT.BusinessLogic.Interfaces;
using SIGCOMT.DTO;
using SIGCOMT.Web.Core;

namespace SIGCOMT.Web.Controllers
{
    public class MenuController : BaseController
    {
        public MenuController(IFormularioBL formularioBL, IPermisoRolBL permisoRolBL, IItemTablaBL itemTablaBL)
            : base(formularioBL, permisoRolBL, itemTablaBL)
        {
        }

        // GET: Menu
        public ActionResult Index()
        {
            List<ModuloDto> formularios = ObtenerFormulariosUsuario();
            return PartialView(formularios);
        }
    }
}