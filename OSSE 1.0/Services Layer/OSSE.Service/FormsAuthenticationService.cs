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
            if (String.IsNullOrEmpty(userName)) 
                throw new ArgumentException("El valor no puede ser NULL ni estar vacío.", "userName");
            
            var usuario = _usuarioBL.Get(p => p.UserName == userName) ?? new Usuario { UserName = MasterConstantes.NoUsuario };

            GenerarTickectAutenticacion(usuario, createPersistentCookie);
        }

        public void SignOut()
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Session.Remove(MasterConstantes.UsuarioSesion);
            HttpContext.Current.Session.Clear();
            FormsAuthentication.SignOut();
        }

        private void GenerarTickectAutenticacion(Usuario usuario, bool createPersistentCookie)
        {
            FormsAuthentication.SetAuthCookie(usuario.UserName, createPersistentCookie);

            var ticket = new FormsAuthenticationTicket(1, usuario.UserName, DateTime.Now, DateTime.Now.AddMinutes(30),
                createPersistentCookie, usuario.Id.ToString());

            var encTicket = FormsAuthentication.Encrypt(ticket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            HttpContext.Current.Session.Add(MasterConstantes.UsuarioSesion, usuario);
            HttpContext.Current.Session.Add(MasterConstantes.IdControlador, 0);
            HttpContext.Current.Response.Cookies.Add(faCookie);
        }
    }
}
