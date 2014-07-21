using System;
using System.Web.Security;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Domain;
using OSSE.Service.Core.Providers;
using OSSE.Service.Interfaces;

namespace OSSE.Service
{
    public class AccountMembershipService : IMembershipService
    {
        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null, null)
        {
        }

        public AccountMembershipService(MembershipProvider provider, IUsuarioBL usuarioBL)
        {
            _provider = provider ?? Membership.Provider;

            var dbMembershipProvider = _provider as DbMembershipProvider;
            if (dbMembershipProvider != null)
            {
                dbMembershipProvider.UsuarioBL = usuarioBL;
            }
            
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("El valor no puede ser NULL ni estar vacío.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("El valor no puede ser NULL ni estar vacío.", "password");

            return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("El valor no puede ser NULL ni estar vacío.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("El valor no puede ser NULL ni estar vacío.", "password");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("El valor no puede ser NULL ni estar vacío.", "email");

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("El valor no puede ser NULL ni estar vacío.", "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("El valor no puede ser NULL ni estar vacío.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("El valor no puede ser NULL ni estar vacío.", "newPassword");

            // El elemento ChangePassword() subyacente iniciará una excepción en lugar de
            // devolver false en determinados escenarios de error.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }

        public void UpdatePasswordDominio(Usuario user, string newPassword)
        {
            _provider.ChangePassword(user.UserName, user.Password, newPassword);
        }
    }
}
