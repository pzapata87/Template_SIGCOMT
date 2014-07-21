using System;
using System.Web;
using System.Web.Security;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Common.Constantes;
using OSSE.Domain;
using OSSE.Service.Interfaces;

namespace OSSE.Service
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        private readonly IUsuarioBL _usuarioBL;

        public FormsAuthenticationService(IUsuarioBL usuarioBL)
        {
            _usuarioBL = usuarioBL;
        }

        public FormsAuthenticationService()
        {

        }

        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("El valor no puede ser NULL ni estar vacío.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
            var usuario = _usuarioBL.Get(p => p.UserName == userName);
            string userData = new Guid().ToString();

            if (usuario == null)
            {
                usuario = new Usuario { UserName = MasterConstantes.NoUsuario };
            }
            else
            {
                userData = usuario.Id.ToString();
            }

            var ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(30), createPersistentCookie, userData);
            var encTicket = FormsAuthentication.Encrypt(ticket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            HttpContext.Current.Session.Add(MasterConstantes.UsuarioSesion, usuario);
            HttpContext.Current.Session.Add(MasterConstantes.IdControlador, 0);
            HttpContext.Current.Response.Cookies.Add(faCookie);
        }

        public void SignOut()
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Remove(MasterConstantes.UsuarioSesion);
            HttpContext.Current.Session.Clear();
            FormsAuthentication.SignOut();
        }
    }
}
