using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Common;
using OSSE.Common.Constantes;
using OSSE.Common.Enum;
using OSSE.Service.Core.Providers;
using OSSE.Service.Interfaces;
using OSSE.Web.Core;
using OSSE.Web.Models;
using Resources;

namespace OSSE.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        #region Propiedades

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        private readonly IUsuarioBL _usuarioBL;
        private readonly IItemTablaBL _itemTablaBL;
        private readonly IFormularioBL _formularioBL;

        #endregion

        #region Constructor
        public AccountController()
        {
        }

        public AccountController(IFormsAuthenticationService formsService, IMembershipService membershipService, IUsuarioBL usuarioBL, IItemTablaBL itemTablaBL, IFormularioBL formularioBL)
        {
            FormsService = formsService;
            MembershipService = membershipService;
            _usuarioBL = usuarioBL;
            _itemTablaBL = itemTablaBL;
            _formularioBL = formularioBL;
        }
        #endregion


        #region Inicializador

        #endregion

        [AllowAnonymous]
        public ActionResult Login()
        {
            var modelo = new AccountModel
            {
                LogOn = new LogOnModel
                {
                    UserName = "",
                    Password = ""
                },
                Reset = new ResetModel()
            };
            return View(modelo);
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LogOnModel model, string returnUrl)
        {
            if (!Request.IsAjaxRequest()) RedirectToAction("Login", "Account", new { area = "" });

            var jsonResponse = new JsonResponse { Success = false };
            try
            {
                if (ModelState.IsValid)
                {
                        if (MembershipService.ValidateUser(model.UserName, model.Password))
                        {
                            jsonResponse.Success = true;
                            jsonResponse.Data = Utils.AbsoluteWebRoot + "Load";
                            FormsService.SignIn(model.UserName, true);
                            FormulariosEnSession();
                        }
                        else
                        {
                            jsonResponse.Message = "Las credenciales especificadas son incorrectas.";
                            Logger.Error(jsonResponse.Message);
                        }
                }
                else
                {
                    jsonResponse.Message = "Las credenciales especificadas son incorrectas.";
                    Logger.Error(jsonResponse.Message);
                }
                return Json(jsonResponse, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError();
            }
        }


        [AllowAnonymous]
        [HttpPost]
        public JsonResult Reset(ResetModel model, string returnUrl)
        {
            if (!Request.IsAjaxRequest()) RedirectToAction("Login", "Account", new { area = "" });

            var jsonResponse = new JsonResponse { Success = false };

            if (ModelState.IsValid)
            {
                try
                {
                    var usuario = _usuarioBL.Get(p => p.Email == model.Email && p.Estado == (int)TipoEstado.Activo);
                    if (usuario != null)
                    {
                        var itemTabla = _itemTablaBL.Get(p => p.TablaId == (int)TipoTabla.Idioma && p.Id == usuario.IdiomaId);
                        string idioma = string.Empty;
                        if (itemTabla != null)
                        {
                            idioma = itemTabla.Nombre.Split('-')[0];
                        }

                        Guid guid = Guid.NewGuid();
                        var password = guid.ToString();
                        usuario.Password = Security.Encriptar(password);
                        string template = CrearTemplateEmail(usuario.UserName, password, idioma);

                        Email.Email.From(ConfigurationManager.AppSettings["EmailPattern"])
                            .To(model.Email)
                            .Subject(Master.ClaveRecuperada)
                            .UsingStringTemplateText(template)
                            .Send();

                        _usuarioBL.Update(usuario);
                        jsonResponse.Success = true;
                        jsonResponse.Message = "Su petición fue procesada con éxito, en breve recibirá un correo con su contraseña.";
                        Logger.Error(jsonResponse.Message);
                    }
                    else
                    {
                        jsonResponse.Message = "El correo electrónico ingresado no está registrado.";
                        Logger.Error(jsonResponse.Message);
                    }
                }
                catch (Exception ex)
                {
                    jsonResponse.Message = "Ocurrio un error, por favor intente de nuevo o más tarde.";
                    Logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                    return MensajeError();
                }
            }
            else
            {
                jsonResponse.Message = "Los datos ingresados son incorrectos";
                Logger.Error(jsonResponse.Message);
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }
       

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            var modelo = new ChangePasswordModel();
            return PartialView(modelo);
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            var jsonResponse = new JsonResponse {Success = false, Message = Usuario.ErrorContraseña};
            if (ModelState.IsValid)
            {
                try
                {
                    if (MembershipService.ValidateUser(UsuarioActual.UserName, model.OldPassword))
                    {
                        var usuario = _usuarioBL.Get(p => p.UserName == UsuarioActual.UserName);
                        usuario.Password = Security.Encriptar(model.NewPassword);
                        _usuarioBL.Update(usuario);
                        jsonResponse.Success = true;
                        jsonResponse.Message = Master.MensajeExito;
                    }
                    else
                    {
                        Logger.Error(jsonResponse.Message);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.Message);
                }
            }
            else {
                //ModelState.AddModelError("", "La Contraseña actual es incorrecta o la nueva contraseña no es válida.");

                var mensajeError = ModelState.Values.FirstOrDefault(p => p.Errors.Count > 0);

                //jsonResponse.Message = Resources.Usuario.Coincidencia;
                jsonResponse.Message = mensajeError.Errors[0].ErrorMessage;
            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/ChangeSuccess

        public ActionResult ChangeSuccess()
        {
            return PartialView();
        }

        #region Métodos Privados
        private string CrearTemplateEmail(string usuario, string password, string idioma)
        {
            string path = "~/Content/Templates/ContactoEmail" + idioma.ToUpper() + ".htm";
            string pathTemplate = Request.MapPath(path);
            var reader = new StreamReader(pathTemplate);
            string template = reader.ReadToEnd();

            template = template.Replace("#Nombre#", usuario);
            template = template.Replace("#Clave#", password);

            return template;
        }

        #endregion

        #region FormularioHelper
        public void FormulariosEnSession()
        {
            var formulariosEnSession = (List<Domain.Formulario>)System.Web.HttpContext.Current.Session[MasterConstantes.Formularios];
            if (formulariosEnSession != null) System.Web.HttpContext.Current.Session.Remove(MasterConstantes.Formularios);

            var nuevosFormularios = _formularioBL.FindAll(p => p.Estado == (int)TipoEstado.Activo).ToList();
            System.Web.HttpContext.Current.Session.Add(MasterConstantes.Formularios, nuevosFormularios);
        }
        #endregion
    }
}
