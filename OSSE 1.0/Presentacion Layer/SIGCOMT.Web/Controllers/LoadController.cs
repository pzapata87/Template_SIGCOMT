using System;
using System.Web.Mvc;
using OSSE.BusinessLogic.Core;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Common.DataTable;
using OSSE.Domain;
using OSSE.DTO;
using OSSE.Web.Core;

namespace OSSE.Web.Controllers
{
    [Authorize]
    public class LoadController : BaseController
    {
        private readonly IItemTablaBL _itemTablaBL;

        public LoadController(IFormularioBL formularioBL, IPermisoRolBL permisoRolBL, IItemTablaBL itemTablaBL) :
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