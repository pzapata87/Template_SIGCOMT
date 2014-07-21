using System.Web.Security;
using OSSE.Domain;

namespace OSSE.Service.Interfaces
{
    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        void UpdatePasswordDominio(Usuario user, string newPassword);
    }
}
