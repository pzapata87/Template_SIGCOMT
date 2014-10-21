using System.Linq;
using System.Web.Mvc;
using SIGCOMT.BusinessLogic.Interfaces;
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

            var formularios = FormularioConverter.GenerateTreeView(modulos, UsuarioActual.IdiomaId);

            return View(formularios);
        }
    }
}
