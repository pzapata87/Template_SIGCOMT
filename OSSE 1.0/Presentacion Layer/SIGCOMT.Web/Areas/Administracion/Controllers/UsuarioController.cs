using System.Web.Mvc;
using SIGCOMT.BusinessLogic.Core;
using SIGCOMT.BusinessLogic.Interfaces;
using SIGCOMT.Common.DataTable;
using SIGCOMT.Domain;
using SIGCOMT.DTO;
using SIGCOMT.Web.Core;

namespace SIGCOMT.Web.Areas.Administracion.Controllers
{
    [Authorize]
    public class UsuarioController : BaseController
    {
        private readonly IUsuarioBL _usuarioBL;

        public UsuarioController(IFormularioBL formularioBL, IPermisoRolBL permisoRolBL, IItemTablaBL itemTablaBL, IUsuarioBL usuarioBL)
             :base(formularioBL, permisoRolBL, itemTablaBL)
        {
            _usuarioBL = usuarioBL;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Listar(GridTable gridTable)
        {
            return ListarJQGrid(new ListParameter<Usuario, UsuarioDto>
            {
                BusinessLogicClass = _usuarioBL,
                Grid = gridTable,
                SelecctionFormat = p => new UsuarioDto
                {
                    Apellido = p.Apellido,
                    Email = p.Email,
                    Estado = p.Estado,
                    Id = p.Id,
                    Idioma = p.IdiomaId.ToString(),
                    Nombre = p.Nombre,
                    Telefono = p.Telefono,
                    UserName = p.UserName
                }
            });
        }
    }
}