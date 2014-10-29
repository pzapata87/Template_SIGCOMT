using System.Linq;
using System.Resources;
using System.Web.Mvc;
using Resources;
using SIGCOMT.BusinessLogic.Interfaces;
using SIGCOMT.Cache;
using SIGCOMT.Common;
using SIGCOMT.Common.Enum;
using SIGCOMT.Converter;
using SIGCOMT.Web.Core;
using SIGCOMT.Web.Core.Aspects;

namespace SIGCOMT.Web.Areas.Administracion.Controllers
{
    [Authorize]
    public class FormularioController : BaseController
    {
        #region Variables Privadas

        private readonly IFormularioBL _formularioBL;

        #endregion

        public FormularioController(IFormularioBL formularioBL, IPermisoRolBL permisoRolBL, IItemTablaBL itemTablaBL)
             :base(formularioBL, permisoRolBL, itemTablaBL)
        {
            _formularioBL = formularioBL;
        }

        [Controller(TipoVerbo = TipoAccionControlador.Get)]
        public ActionResult Index()
        {
            var modulos = _formularioBL.FindAll(p => p.Estado == (int) TipoEstado.Activo).ToList();

            var formularios = FormularioConverter.DomainToDtoFormulario(modulos, UsuarioActual.IdiomaId);

            return View(formularios);
        }

        [Controller(TipoVerbo = TipoAccionControlador.Post)]
        [HttpPost]
        public JsonResult ObtenerPermiso(int formularioId)
        {
            var response = new JsonResponse {Success = true};
            var permisos = GlobalParameters.PermisoFormularioList[formularioId];
            response.Data = FormularioConverter.ObtenerPermisosFormulario(permisos);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
