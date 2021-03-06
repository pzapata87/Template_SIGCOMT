﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Web.Mvc;
using log4net;
using Microsoft.Reporting.WebForms;
using OSSE.BusinessLogic.Core;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Common;
using OSSE.Common.Constantes;
using OSSE.Common.Enum;
using OSSE.Common.JQGrid;
using OSSE.Converter;
using OSSE.Domain;
using OSSE.Domain.Core;
using OSSE.Web.Models;

namespace OSSE.Web.Core
{
    [HandleError]
    public class BaseController : Controller
    {
        #region Variables Privadas

        protected static readonly ILog Logger = LogManager.GetLogger(string.Empty);
        private readonly IFormularioBL _formularioBL;
        private readonly IPermisoRolBL _permisoRolBL;
        private readonly IItemTablaBL _itemTablaBL;

        #endregion

        #region Constructor

        public BaseController()
        {
        }

        public BaseController(IFormularioBL formularioBL, IPermisoRolBL permisoRolBL, IItemTablaBL itemTablaBL)
        {
            _formularioBL = formularioBL;
            _permisoRolBL = permisoRolBL;
            _itemTablaBL = itemTablaBL;

            if (UsuarioActual != null)
            {
                if (UsuarioActual.UserName == MasterConstantes.NoUsuario)
                {
                    RedirectToAction("LogOff", "Account", new { area = "" });
                    return;
                }

                ViewData[MasterConstantes.UsuarioSesion] = UsuarioActual;
                //ViewData[MasterConstantes.FechaSistema] = FechaSistema;
                ViewData[MasterConstantes.UsuarioActual] = UsuarioActual.UserName;
                ViewData[MasterConstantes.Empresa] = "OSSE";
                ViewData[MasterConstantes.RUC] = "20508473657";
                ViewData[MasterConstantes.Direccion] = "San Isidro";
                ViewData[MasterConstantes.Telefono] = " 01 2641251|2641214";
                ViewData[MasterConstantes.Email] = "osse@hotmail.com";
                ViewData[MasterConstantes.IdUsuarioActual] = UsuarioActual.Id;

                ItemTabla idioma = itemTablaBL.Get(p => p.TablaId == (int)TipoTabla.Idioma && p.Id == UsuarioActual.IdiomaId);

                if (idioma != null)
                {
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(idioma.Nombre);
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(idioma.Nombre);
                    Idioma = Thread.CurrentThread.CurrentUICulture.ToString().Split('-')[0];
                }

                ViewData[MasterConstantes.Idioma] = Idioma;
            }

            var formulariosEnSession = (List<Formulario>)System.Web.HttpContext.Current.Session[MasterConstantes.Formularios];
            if (formulariosEnSession != null) 
                System.Web.HttpContext.Current.Session.Remove(MasterConstantes.Formularios);

            var nuevosFormularios = formularioBL.FindAll(p => p.Estado == (int)TipoEstado.Activo).ToList();
            System.Web.HttpContext.Current.Session.Add(MasterConstantes.Formularios, nuevosFormularios);
        }

        #endregion

        #region Propiedades

        protected Usuario UsuarioActual
        {
            get { return (Usuario)System.Web.HttpContext.Current.Session[MasterConstantes.UsuarioSesion]; }
            set { System.Web.HttpContext.Current.Session.Add(MasterConstantes.UsuarioSesion, value); }
        }

        public int IdControlador
        {
            get { return (int)System.Web.HttpContext.Current.Session[MasterConstantes.IdControlador]; }
            set { System.Web.HttpContext.Current.Session.Add(MasterConstantes.IdControlador, value); }
        }

        public static string Idioma { get; set; }

        #endregion

        #region Métodos

        #region Paginacion

        protected GenericDouble<JQgrid, T> Listar<T>(Func<Expression<Func<T, bool>>, int> countMethod,
            Func<JQGridParameters<T>, IQueryable<T>> listMethod, JQGridParameters<T> parameters) where T : class
        {
            var jqgrid = new JQgrid();
            IList<T> list;

            try
            {
                int totalPages = 0;

                var count = countMethod(parameters.WhereFilter);

                if (count > 0 && parameters.AmountRows > 0)
                {
                    if (count % parameters.AmountRows > 0)
                    {
                        totalPages = count / parameters.AmountRows + 1;
                    }
                    else
                    {
                        totalPages = count / parameters.AmountRows;
                    }

                    totalPages = totalPages == 0 ? 1 : totalPages;
                }

                parameters.CurrentPage = parameters.CurrentPage > totalPages ? totalPages : parameters.CurrentPage;

                parameters.Start = parameters.AmountRows * parameters.CurrentPage - parameters.AmountRows;
                if (parameters.Start < 0)
                {
                    parameters.Start = 0;
                }

                jqgrid.Total = totalPages;
                jqgrid.Page = parameters.CurrentPage;
                jqgrid.Records = count;
                jqgrid.Start = parameters.Start;

                list = listMethod(parameters).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return new GenericDouble<JQgrid, T>(jqgrid, list);
        }

        protected JsonResult ListarJQGrid<T>(ListJQGridParameter<T> parameter) where T : EntityBase
        {
            try
            {
                GridTable grid = parameter.Grid;

                grid.Page = (grid.Page == 0) ? 1 : grid.Page;
                grid.Rows = (grid.Rows == 0) ? 100 : grid.Rows;

                var where = UtilsComun.ConvertToLambda<T>(grid.Filters).And(parameter.FiltrosAdicionales ?? (q => true));

                var parameters = new JQGridParameters<T>
                {
                    ColumnOrder = grid.Sidx,
                    CurrentPage = grid.Page,
                    OrderType = (TipoOrden)Enum.Parse(typeof(TipoOrden), grid.Sord, true),
                    WhereFilter = where,
                    AmountRows = grid.Rows
                };

                GenericDouble<JQgrid, T> generic = Listar(parameter.CountMethod, parameter.ListMethod, parameters);
                generic.Value.Rows = generic.List.Select(parameter.SelecctionFormat).ToArray();

                return Json(generic.Value);
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
                var aux = new List<PermisoRol>();

                var roles = UsuarioActual.RolUsuarioList.ToList();
                foreach (var rol in roles)
                {
                    RolUsuario rolUsuarioActual = rol;
                    aux.AddRange(_permisoRolBL.GetAll(p => p.RolId == rolUsuarioActual.RolId && p.FormularioId == idFormulario));
                }

                IEnumerable<PermisoRol> permisos = aux.Distinct();

                var model = new FormularioModel(formulario, permisos);
                return PartialView(view, model);
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
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];

            Logger.Error(string.Format("Controlador:{0}  Action:{1}  Mensaje:{2}", controllerName, actionName, UtilsComun.GetExceptionMessage(filterContext.Exception)));
            filterContext.Result = View("Error");
        }

        protected JsonResult MensajeError(string mensaje = "Ocurrio un error al cargar...")
        {
            Response.StatusCode = 404;
            return Json(new JsonResponse { Message = mensaje }, JsonRequestBehavior.AllowGet);
        }

        protected void LogError(Exception exception)
        {
            Logger.Error(string.Format("Mensaje: {0} Trace: {1}", exception.Message, exception.StackTrace));
        }

        #endregion Control Error

        #region GenerarTreeView

        [HttpPost]
        public virtual JsonResult ListarTreView()
        {
            try
            {
                if (UsuarioActual == null)
                    return Json("");

                var modulos = _formularioBL.Formularios(UsuarioActual);
                var modulosUsuario = FormularioConverter.GenerateTreeView(modulos, UsuarioActual.IdiomaId);

                return Json(modulosUsuario);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError();
            }
        }

        #endregion

        #region Listado ItemTabla

        /// <summary>
        /// Obtiene un listado de ItemTabla dado el idTipoTabla
        /// </summary>
        /// <returns></returns>
        public List<ItemTabla> ItemTablaList(int idTipoTabla)
        {
            return _itemTablaBL.FindAll(p => p.TablaId == idTipoTabla).ToList();
        }

        #endregion

        public new RedirectToRouteResult RedirectToAction(string action, string controller) 
        {
            return base.RedirectToAction(action, controller);
        }

        public new JsonResult Json(object data, JsonRequestBehavior behavior)
        {
            return base.Json(data, behavior);
        }

        #region Reportes

        /// <summary>
        /// Renderiza un reporte con sus datos
        /// </summary>
        /// <param name="report">Nombre del reporte</param>
        /// <param name="ds">Conjunto de Datos</param>
        /// <param name="data">Datos</param>
        /// <param name="formato">Formato PDF o Excel</param>
        /// <param name="parametros"></param>
        public void RenderReport(string report, string ds, object data, string formato, ReportParameter[] parametros = null)
        {
            string reportPath = Server.MapPath(string.Format("~/Reports/{0}.rdlc", report));
            var localReport = new LocalReport { ReportPath = reportPath };
            var reportDataSource = new ReportDataSource(ds, data);

            localReport.DataSources.Add(reportDataSource);
            if (parametros != null)
                localReport.SetParameters(parametros);

            string reportType;
            string deviceInfo = string.Empty;

            switch (formato)
            {
                case "PDF":
                    reportType = "PDF";
                    deviceInfo =
                        string.Format(
                            "<DeviceInfo><OutputFormat>{0}</OutputFormat><PageWidth>8.9in</PageWidth><PageHeight>11in</PageHeight><MarginTop>0.2in</MarginTop><MarginLeft>0.2in</MarginLeft><MarginRight>0.2in</MarginRight><MarginBottom>0.2in</MarginBottom></DeviceInfo>",
                            reportType);
                    break;
                case "EXCEL":
                    reportType = "Excel";

                    break;
                default:
                    return;
            }

            string mimeType;
            string encoding;
            string fileNameExtension;
            Warning[] warnings;
            string[] streams;

            byte[] renderedBytes = localReport.Render(reportType, deviceInfo, out mimeType, out encoding,
                                                      out fileNameExtension, out streams, out warnings);

            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition",
                                                   string.Format("attachment; filename={0}.{1}",
                                                                 UtilsComun.GetReporteName(report), fileNameExtension));
            Response.BinaryWrite(renderedBytes);
            Response.End();
        }

        #endregion

        #endregion
    }
}
