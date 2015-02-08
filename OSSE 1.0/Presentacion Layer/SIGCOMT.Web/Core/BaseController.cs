using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using log4net;
using SIGCOMT.BusinessLogic.Core;
using SIGCOMT.BusinessLogic.Interfaces;
using SIGCOMT.Common;
using SIGCOMT.Common.Constantes;
using SIGCOMT.Common.DataTable;
using SIGCOMT.Common.Enum;
using SIGCOMT.Common.FiltersRules;
using SIGCOMT.Converter;
using SIGCOMT.Domain;
using SIGCOMT.Domain.Core;
using SIGCOMT.DTO;

namespace SIGCOMT.Web.Core
{
    [HandleError]
    public class BaseController : Controller
    {
        #region Variables Privadas

        protected static readonly ILog Logger = LogManager.GetLogger(string.Empty);
        private readonly IFormularioBL _formularioBL;
        private readonly IItemTablaBL _itemTablaBL;
        private readonly IPermisoFormularioRolBL _permisoRolBL;

        #endregion

        #region Constructor

        public BaseController()
        {
        }

        public BaseController(IFormularioBL formularioBL, IPermisoFormularioRolBL permisoRolBL, IItemTablaBL itemTablaBL)
        {
            _formularioBL = formularioBL;
            _permisoRolBL = permisoRolBL;
            _itemTablaBL = itemTablaBL;

            if (UsuarioActual != null)
            {
                if (UsuarioActual.UserName == MasterConstantes.NoUsuario)
                {
                    RedirectToAction("LogOut", "Account", new { area = "" });
                    return;
                }

                ViewData[MasterConstantes.UsuarioSesion] = UsuarioActual;
                ViewData[MasterConstantes.UsuarioActual] = UsuarioActual.UserName;
                ViewData[MasterConstantes.Empresa] = "SIGCOMT";
                ViewData[MasterConstantes.IdUsuarioActual] = UsuarioActual.Id;
                
            }
        }

        #endregion

        #region Propiedades

        protected Usuario UsuarioActual
        {
            get { return (Usuario) System.Web.HttpContext.Current.Session[MasterConstantes.UsuarioSesion]; }
            set { System.Web.HttpContext.Current.Session.Add(MasterConstantes.UsuarioSesion, value); }
        }

        public int IdControlador
        {
            get { return (int) System.Web.HttpContext.Current.Session[MasterConstantes.IdControlador]; }
            set { System.Web.HttpContext.Current.Session.Add(MasterConstantes.IdControlador, value); }
        }

        public static string Idioma { get; set; }

        #endregion

        #region Métodos

        #region Cultura

        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            if (UsuarioActual == null || _itemTablaBL == null)
                return base.BeginExecuteCore(callback, state);


            var idioma = _itemTablaBL.Get(p => p.TablaId == (int)TipoTabla.Idioma && p.Id == UsuarioActual.IdiomaId);

            if (idioma != null)
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(idioma.Nombre);
                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                Idioma = idioma.Nombre.Split('-')[0];

                ViewData[MasterConstantes.Idioma] = Idioma;
            }

            return base.BeginExecuteCore(callback, state);
        }

        #endregion

        #region Paginacion

        protected JsonResult ListarJQGrid<T, TResult>(ListParameter<T, TResult> configuracionListado)
            where T : EntityBase where TResult : class
        {
            try
            {
                GridTable grid = configuracionListado.Grid;

                Expression<Func<T, bool>> where =
                     LambdaFilterManager.ConvertToLambda<T>(grid.Columns, grid.Search, grid.Homologaciones)
                        .And(configuracionListado.FiltrosAdicionales ?? (q => true));

                var count = configuracionListado.CountMethod(where);
                OrderColumn ordenamiento = grid.Order.First();

                var currentPage = (grid.Start/grid.Length);
                var parametroFiltro = new FilterParameters<T>
                {
                    ColumnOrder = grid.Columns[ordenamiento.Column].Name,
                    CurrentPage = (currentPage >= 0 ? currentPage : 0) + 1,
                    OrderType =
                        ordenamiento.Dir != null
                            ? (TipoOrden) Enum.Parse(typeof (TipoOrden), ordenamiento.Dir, true)
                            : TipoOrden.Asc,
                    WhereFilter = where,
                    AmountRows = grid.Length > 0 ? grid.Length : count
                };

                int totalPages = 0;

                if (count > 0 && parametroFiltro.AmountRows > 0)
                {
                    if (count%parametroFiltro.AmountRows > 0)
                    {
                        totalPages = count/parametroFiltro.AmountRows + 1;
                    }
                    else
                    {
                        totalPages = count/parametroFiltro.AmountRows;
                    }

                    totalPages = totalPages == 0 ? 1 : totalPages;
                }

                parametroFiltro.CurrentPage = parametroFiltro.CurrentPage > totalPages
                    ? totalPages
                    : parametroFiltro.CurrentPage;
                parametroFiltro.Start = grid.Start;

                List<TResult> respuestaList =
                    configuracionListado.ListMethod(parametroFiltro)
                        .ToList()
                        .Select(configuracionListado.SelecctionFormat).ToList();

                var responseData = new DataTableResponse<TResult>
                {
                    data = respuestaList,
                    recordsFiltered = count,
                    recordsTotal = count
                };

                return Json(responseData);
            }
            catch (Exception ex)
            {
                LogError(ex);
                return MensajeError();
            }
        }

        #endregion Paginacion

        #region Vista

        protected ActionResult BasePartialView(string view, string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    return MensajeError("Identificador de formulario invalido.");
                }

                int idFormulario = Convert.ToInt32(id);

                Formulario formulario = _formularioBL.GetById(idFormulario);
                var aux = new List<PermisoFormularioRol>();

                List<RolUsuario> roles = UsuarioActual.RolUsuarioList.ToList();
                foreach (RolUsuario rol in roles)
                {
                    RolUsuario rolUsuarioActual = rol;
                    aux.AddRange(_permisoRolBL.GetAll(p => p.RolId == rolUsuarioActual.RolId && p.FormularioId == idFormulario));
                }

                IEnumerable<PermisoFormularioRol> permisos = aux.Distinct();

                PermisoFormularioDto permisosFormulario = FormularioConverter.ObtenerPermisosFormulario(formulario, permisos);
                return PartialView(view, permisosFormulario);
            }
            catch
            {
                return MensajeError();
            }
        }

        #endregion Vista

        #region Control Error

        protected override void OnException(ExceptionContext filterContext)
        {
            Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            object controllerName = filterContext.RouteData.Values["controller"];
            object actionName = filterContext.RouteData.Values["action"];

            Logger.Error(string.Format("Controlador:{0}  Action:{1}  Mensaje:{2}", controllerName, actionName,
                Helper.GetExceptionMessage(filterContext.Exception)));
            filterContext.Result = View("Error");
        }

        protected JsonResult MensajeError(string mensaje = "Ocurrio un error al cargar...")
        {
            Response.StatusCode = 404;
            return Json(new JsonResponse {Message = mensaje}, JsonRequestBehavior.AllowGet);
        }

        protected void LogError(Exception exception)
        {
            Logger.Error(string.Format("Mensaje: {0} Trace: {1}", exception.Message, exception.StackTrace));
        }

        #endregion Control Error

        #region GenerarTreeView

        public virtual List<ModuloDto> ObtenerFormulariosUsuario()
        {
            try
            {
                if (UsuarioActual == null)
                    return null;

                var modulos = _formularioBL.Formularios(UsuarioActual);
                return FormularioConverter.GenerateTreeView(modulos, UsuarioActual.IdiomaId);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
            }

            return null;
        }

        #endregion

        public new RedirectToRouteResult RedirectToAction(string action, string controller, object routeValues)
        {
            return base.RedirectToAction(action, controller, routeValues);
        }

        public new JsonResult Json(object data, JsonRequestBehavior behavior)
        {
            return base.Json(data, behavior);
        }

        #endregion
    }
}