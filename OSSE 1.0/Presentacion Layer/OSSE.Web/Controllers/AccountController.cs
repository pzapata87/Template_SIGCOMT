using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Common.Constantes;
using OSSE.Common.Enum;
using OSSE.Domain;
using OSSE.DTO;
using OSSE.Web.Core;

namespace OSSE.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        #region Propiedades

        private readonly IFormularioBL _formularioBL;
        private readonly IUsuarioBL _usuarioBL;

        #endregion

        #region Constructor

        public AccountController()
        {
        }

        public AccountController(IUsuarioBL usuarioBL, IFormularioBL formularioBL)
        {
            _usuarioBL = usuarioBL;
            _formularioBL = formularioBL;
        }

        #endregion

        #region Métodos Públicos

        [AllowAnonymous]
        public ActionResult Login()
        {
            var login = new LogInDto
            {
                UserName = "",
                Password = ""
            };

            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LogInDto model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Usuario usuarioLogueado = _usuarioBL.ValidateUser(model.UserName, model.Password);
                    if (usuarioLogueado != null)
                    {
                        GenerarTickectAutenticacion(usuarioLogueado, true);
                        FormulariosEnSession();

                        return RedirectToAction("Index", "Load");
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Mensaje: {0} Trace: {1}", ex.Message, ex.StackTrace));
                return MensajeError();
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region FormularioHelper

        public void FormulariosEnSession()
        {
            var formulariosEnSession = (List<Formulario>) System.Web.HttpContext.Current.Session[MasterConstantes.Formularios];
            if (formulariosEnSession != null) System.Web.HttpContext.Current.Session.Remove(MasterConstantes.Formularios);

            List<Formulario> nuevosFormularios = _formularioBL.FindAll(p => p.Estado == (int) TipoEstado.Activo).ToList();
            System.Web.HttpContext.Current.Session.Add(MasterConstantes.Formularios, nuevosFormularios);
        }

        private void GenerarTickectAutenticacion(Usuario usuario, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(usuario.UserName, createPersistentCookie);

            var ticket = new FormsAuthenticationTicket(1, usuario.UserName, DateTime.Now, DateTime.Now.AddMinutes(30),
                createPersistentCookie, usuario.Id.ToString());

            string encTicket = FormsAuthentication.Encrypt(ticket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            System.Web.HttpContext.Current.Session.Add(MasterConstantes.UsuarioSesion, usuario);
            System.Web.HttpContext.Current.Session.Add(MasterConstantes.IdControlador, 0);
            System.Web.HttpContext.Current.Response.Cookies.Add(faCookie);
        }

        #endregion
    }
}