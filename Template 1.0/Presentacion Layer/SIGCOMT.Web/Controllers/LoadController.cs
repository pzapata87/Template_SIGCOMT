using System;
using System.Web.Mvc;
using SIGCOMT.BusinessLogic.Core;
using SIGCOMT.BusinessLogic.Interfaces;
using SIGCOMT.Common.DataTable;
using SIGCOMT.Domain;
using SIGCOMT.DTO;
using SIGCOMT.Web.Core;

namespace SIGCOMT.Web.Controllers
{
    [Authorize]
    public class LoadController : BaseController
    {
        private readonly IItemTablaBL _itemTablaBL;

        public LoadController(IFormularioBL formularioBL, IPermisoFormularioRolBL permisoRolBL, IItemTablaBL itemTablaBL) :
            base(formularioBL, permisoRolBL, itemTablaBL)
        {
            _itemTablaBL = itemTablaBL;
        }


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Listar(GridTable gridTable)
        {
            return ListarJQGrid(new ListParameter<ItemTabla, ItemTablaDto>
            {
                BusinessLogicClass = _itemTablaBL,
                Grid = gridTable,
                SelecctionFormat = p => new ItemTablaDto
                {
                    Descripcion = p.Descripcion,
                    Nombre = p.Nombre,
                    Valor = p.Valor
                }
            });
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