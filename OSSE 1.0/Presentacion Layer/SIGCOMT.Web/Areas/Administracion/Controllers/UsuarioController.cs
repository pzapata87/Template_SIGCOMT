using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SIGCOMT.BusinessLogic.Core;
using SIGCOMT.BusinessLogic.Interfaces;
using SIGCOMT.Cache;
using SIGCOMT.Common;
using SIGCOMT.Common.DataTable;
using SIGCOMT.Common.Enum;
using SIGCOMT.Converter;
using SIGCOMT.Domain;
using SIGCOMT.DTO;
using SIGCOMT.Web.Core;
using SIGCOMT.Web.Core.Aspects;

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
            var listaIdiomas = new List<ValorHomologacion>();

            GlobalParameters.Idiomas.ToList().ForEach(p => listaIdiomas.Add(new ValorHomologacion
            {
                ValorHomologado = Convert.ToString(p.Key),
                ValorReal = p.Value
            }));

            var parametrosListado = new Dictionary<string, List<ValorHomologacion>> {{"IdiomaId", listaIdiomas}};

            return View(parametrosListado);
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
                    Id = p.Id,
                    UserName = p.UserName,
                    Email = p.Email,
                    Nombre = p.Nombre,
                    Apellido = p.Apellido,
                    Telefono = p.Telefono,
                    Idioma = GlobalParameters.Idiomas[p.IdiomaId],
                    Estado = p.Estado
                }
            });
        }

        [Controller(TipoVerbo = TipoAccionControlador.Get)]
        public ActionResult Edit(int id)
        {
            ViewBag.Accion = "Edit";

            var usuarioActual = UsuarioConverter.DomainToDto(_usuarioBL.GetById(id));
            return View(usuarioActual);
        }

        [Controller(TipoVerbo = TipoAccionControlador.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Edit(UsuarioDto usuarioDto)
        {
            var response = new JsonResponse {Success = false};

            var entityTemp =
                        _usuarioBL.Get(
                            p => p.UserName == usuarioDto.UserName && p.Id != usuarioDto.Id && p.Estado == (int)TipoEstado.Activo);

            if (entityTemp == null)
            {
                var usuarioDomain = _usuarioBL.GetById(usuarioDto.Id);
                UsuarioConverter.DtoToDomain(usuarioDomain, usuarioDto);

                _usuarioBL.Update(usuarioDomain);

                response.Message = "Se actualizó el usuario correctamente";
                response.Success = true;
            }
            else
            {
                response.Message = "Ya existe el nombre de usuario";
                response.Success = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Controller(TipoVerbo = TipoAccionControlador.Get)]
        public ActionResult Crear()
        {
            ViewBag.Accion = "Crear";
            var usuarioDto = new UsuarioDto();

            return View("Edit", usuarioDto);
        }

        [Controller(TipoVerbo = TipoAccionControlador.Post)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Crear(UsuarioDto usuarioDto)
        {
            var response = new JsonResponse { Success = false };

            var entityTemp =
                        _usuarioBL.Get(
                            p => p.UserName == usuarioDto.UserName && p.Estado == (int)TipoEstado.Activo);

            if (entityTemp == null)
            {
                var usuarioDomain = new Usuario { Estado = (int)TipoEstado.Activo };

                UsuarioConverter.DtoToDomain(usuarioDomain, usuarioDto);

                _usuarioBL.Add(usuarioDomain);

                response.Message = "Se registró el usuario correctamente";
                response.Success = true;
            }
            else
            {
                response.Message = "Ya existe el nombre de usuario";
                response.Success = false;
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [Controller(TipoVerbo = TipoAccionControlador.Post)]
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            var usuarioDomain = _usuarioBL.GetById(id);
            usuarioDomain.Estado = (int)TipoEstado.Inactivo;

            _usuarioBL.Update(usuarioDomain);

            var jsonResponse = new JsonResponse { Success = true, Message = "Se eliminó satisfactoriamente el usuario" };
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [Controller(TipoVerbo = TipoAccionControlador.Post)]
        [HttpPost]
        public JsonResult ObtenerPermisoFormulario(int usuarioId)
        {
            var response = new JsonResponse { Success = false };
            var user = _usuarioBL.GetById(usuarioId);

            if (user != null)
            {
                response.Data = UsuarioConverter.PermisosFormulario(user.PermisoUsuarioList, user.RolUsuarioList);
            }
            else
            {
                response.Message = "Usuario no existe";
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}