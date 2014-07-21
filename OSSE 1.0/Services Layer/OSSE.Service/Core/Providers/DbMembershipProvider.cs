using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Web;
using System.Web.Security;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Common.Enum;
using OSSE.Domain;

namespace OSSE.Service.Core.Providers
{
    public class DbMembershipProvider : MembershipProvider
    {
        #region Constructor

        public DbMembershipProvider(IUsuarioBL usuarioBL)
        {
            UsuarioBL = usuarioBL;
        }

        public DbMembershipProvider()
        {
            //this.usuarioBL = new UsuarioBL();
        }

        #endregion

        #region Variables

        private const int PasswordSize = 14;

        private bool _enablePasswordRetrieval;
        private bool _enablePasswordReset;
        private bool _requiresQuestionAndAnswer;
        private bool _requiresUniqueEmail;
        private int _maxInvalidPasswordAttempts;
        private int _passwordAttemptWindow;
        private int _minRequiredPasswordLength;
        private int _minRequiredNonalphanumericCharacters;
        private string _passwordStrengthRegularExpression;
        private string _appName;
        private MembershipPasswordFormat _passwordFormat;
        private string _sqlConnectionString;

        #endregion Variables

        #region Propiedades
        //private readonly IUsuarioBL usuarioBL;
        public IUsuarioBL UsuarioBL { get; set; }

        public override string ApplicationName
        {
            get { return _appName; }
            set { _appName = value; }
        }

        public override bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return _enablePasswordRetrieval; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return _maxInvalidPasswordAttempts; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return _minRequiredNonalphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { return _passwordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return _passwordFormat; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return _passwordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return _requiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return _requiresUniqueEmail; }
        }

        #endregion Propiedades

        #region Métodos Publicos

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            if (String.IsNullOrEmpty(name))
                name = "DbMembershipProvider";
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "MembershipSqlProvider_description");
            }
            base.Initialize(name, config);

            _enablePasswordRetrieval = Security.GetBooleanValue(config, "enablePasswordRetrieval", false);
            _enablePasswordReset = Security.GetBooleanValue(config, "enablePasswordReset", true);
            _requiresQuestionAndAnswer = Security.GetBooleanValue(config, "requiresQuestionAndAnswer", true);
            _requiresUniqueEmail = Security.GetBooleanValue(config, "requiresUniqueEmail", true);
            _maxInvalidPasswordAttempts = Security.GetIntValue(config, "maxInvalidPasswordAttempts", 5, false, 0);
            _passwordAttemptWindow = Security.GetIntValue(config, "passwordAttemptWindow", 10, false, 0);
            _minRequiredPasswordLength = Security.GetIntValue(config, "minRequiredPasswordLength", 6, false, 128);
            _minRequiredNonalphanumericCharacters = Security.GetIntValue(config, "minRequiredNonalphanumericCharacters", 1, true, 128);

            _passwordStrengthRegularExpression = config["passwordStrengthRegularExpression"];
            if (_passwordStrengthRegularExpression != null)
            {
                _passwordStrengthRegularExpression = _passwordStrengthRegularExpression.Trim();
                if (_passwordStrengthRegularExpression.Length != 0)
                {
                    //try
                    //{
                    //    //var regex = new Regex(_passwordStrengthRegularExpression);
                    //}
                    //catch (ArgumentException e)
                    //{
                    //    throw new ProviderException(e.Message, e);
                    //}
                }
            }
            else
            {
                _passwordStrengthRegularExpression = string.Empty;
            }
            if (_minRequiredNonalphanumericCharacters > _minRequiredPasswordLength)
                throw new HttpException("MinRequiredNonalphanumericCharacters_can_not_be_more_than_MinRequiredPasswordLength");

            var temp = config["connectionStringName"];
            if (string.IsNullOrEmpty(temp))
            {
                throw new ProviderException("Connection_name_not_specified");
            }
            _sqlConnectionString = temp;
            if (string.IsNullOrEmpty(_sqlConnectionString))
            {
                throw new ProviderException("Connection_string_not_found");
            }

            Security.GetIntValue(config, "commandTimeout", 30, true, 0);
            _appName = config["applicationName"];

            if (string.IsNullOrEmpty(_appName))
            {
                _appName = "/";
            }

            if (_appName.Length > 256)
            {
                throw new ProviderException("Provider_application_name_too_long");
            }

            var strTemp = config["passwordFormat"] ?? "Hashed";

            switch (strTemp)
            {
                case "Clear":
                    _passwordFormat = MembershipPasswordFormat.Clear;
                    break;
                case "Encrypted":
                    _passwordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Hashed":
                    _passwordFormat = MembershipPasswordFormat.Hashed;
                    break;
                default:
                    throw new ProviderException("Provider_bad_password_format");
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed && EnablePasswordRetrieval)
                throw new ProviderException("Provider_can_not_retrieve_hashed_password");

            config.Remove("connectionStringName");
            config.Remove("enablePasswordRetrieval");
            config.Remove("enablePasswordReset");
            config.Remove("requiresQuestionAndAnswer");
            config.Remove("applicationName");
            config.Remove("requiresUniqueEmail");
            config.Remove("maxInvalidPasswordAttempts");
            config.Remove("passwordAttemptWindow");
            config.Remove("commandTimeout");
            config.Remove("passwordFormat");
            config.Remove("name");
            config.Remove("minRequiredPasswordLength");
            config.Remove("minRequiredNonalphanumericCharacters");
            config.Remove("passwordStrengthRegularExpression");
            if (config.Count > 0)
            {
                string attribUnrecognized = config.GetKey(0);
                if (!String.IsNullOrEmpty(attribUnrecognized))
                    throw new ProviderException("Provider_unrecognized_attribute, attribUnrecognized");
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            Usuario user = UsuarioBL.Get(p => p.UserName == username && p.Estado == (int)TipoEstado.Activo);

            if (user == null)
            {
                return false;
            }

            if (user.UserName == null)
            {
                return false;
            }

            user.Password = Security.Encriptar(newPassword);
            UsuarioBL.Update(user);

            return true;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            // Validate username/password
            var args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(email) != string.Empty)
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            // Check whether user with passed username already exists
            Usuario user;
            try
            {
                user = UsuarioBL.Get(p => p.UserName == username && p.Estado == (int)TipoEstado.Activo);
            }
            catch
            {
                user = null;
            }

            if (user.UserName == null)
            {
                try
                {
                    UsuarioBL.Add(new Usuario
                    {
                        Id = 0,
                        UserName = username,
                        Email = email,
                        Password = Security.Encriptar(password)
                    });

                    status = MembershipCreateStatus.Success;
                }
                catch
                {
                    status = MembershipCreateStatus.UserRejected;
                }

                return GetUser(username, false);
            }

            status = MembershipCreateStatus.DuplicateUserName;

            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            try
            {
                Usuario user = UsuarioBL.Get(p => p.UserName == username && p.Estado == (int)TipoEstado.Activo);
                return Security.Desencriptar(user.Password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            Usuario user = UsuarioBL.Get(p => p.UserName == username && p.Estado == (int)TipoEstado.Activo);
            if (userIsOnline)
            {
                //user.LastLogin = true;

                UsuarioBL.Update(user);
            }
            return UserMapper(Name, user);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (providerUserKey == null)
                throw new ArgumentNullException();

            int userid = int.Parse(providerUserKey.ToString());
            Usuario user = UsuarioBL.GetById(userid);
            if (user == null) return null;

            if (userIsOnline)
            {
                //user.LastLogin = true;
                UsuarioBL.Update(user);
            }
            return UserMapper(Name, user);
        }

        public override string GetUserNameByEmail(string email)
        {
            Usuario user = UsuarioBL.Get(p => p.Email == email);
            return user.UserName ?? string.Empty;
        }

        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordReset)
            {
                throw new NotSupportedException("Not_configured_to_support_password_resets");
            }

            Security.CheckParameter(ref username, true, true, true, 40, "username");

            Usuario user = UsuarioBL.Get(p => p.UserName == username && p.Estado == (int)TipoEstado.Activo);
            string newPassword = GeneratePassword();
            var e = new ValidatePasswordEventArgs(username, newPassword, false);
            OnValidatingPassword(e);

            if (e.Cancel)
            {
                throw e.FailureInformation ?? new ProviderException("Membership_Custom_Password_Validation_Failure");
            }

            user.Password = Security.Encriptar(newPassword);
            UsuarioBL.Update(user);
            return newPassword;
        }

        public override bool UnlockUser(string userName)
        {
            Security.CheckParameter(ref userName, true, true, true, 40, "username");
            try
            {
                Usuario user = UsuarioBL.Get(p => p.UserName == userName);
                if (user == null) return false;

                //user.IsLockedOut = false;

                UsuarioBL.Update(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            Usuario usr;
            return ValidateParameter(ref username, true, true, true, 100) &&
                   ValidateParameter(ref password, true, true, false, 100) &&
                   CheckPassword(username, password, false, out usr);
        }

        public virtual string GeneratePassword()
        {
            return Membership.GeneratePassword(
                      MinRequiredPasswordLength < PasswordSize ? PasswordSize : MinRequiredPasswordLength,
                      MinRequiredNonAlphanumericCharacters);
        }

        public MembershipUser UserMapper(string pname, Usuario user)
        {
            return new MembershipUser(pname,
                                      user.UserName,
                                      user.Id,
                                      user.Email, //user.Email,
                                      "",
                                      "",
                                      true,
                                      false, //Bloqueado
                                      DateTime.Now,
                                      DateTime.Now,
                                      DateTime.Now,
                                      DateTime.Now,
                                      DateTime.Now);
        }

        #endregion Métodos Publicos

        #region Métodos Privados

        private bool CheckPassword(string username, string password, bool updateLastLoginActivityDate, out Usuario user)
        {
            user = UsuarioBL.Get(p => p.UserName == username && p.Estado == (int)TipoEstado.Activo);

            if (user == null)
            {
                return false;
            }

            if (user.UserName == null)
            {
                return false;
            }

            var enc = Security.Encriptar(password);

            if (enc == user.Password)
            {
                if (updateLastLoginActivityDate)
                {
                    //user.LastLogin = true;

                    UsuarioBL.Update(user);
                }
                return true;
            }
            return false;
        }

        internal static bool ValidateParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize)
        {
            if (param != null)
            {
                param = param.Trim();
                return ((!checkIfEmpty || param.Length >= 1) &&
                        (maxSize <= 0 || param.Length <= maxSize)) &&
                       (!checkForCommas || !param.Contains(","));
            }
            return !checkForNull;
        }

// ReSharper disable once UnusedMember.Local
        private static string GetExceptionText(int status)
        {
            string key;
            switch (status)
            {
                case 0:
                    return String.Empty;
                case 1:
                    key = "Membership_UserNotFound";
                    break;
                case 2:
                    key = "Membership_WrongPassword";
                    break;
                case 3:
                    key = "Membership_WrongAnswer";
                    break;
                case 4:
                    key = "Membership_InvalidPassword";
                    break;
                case 5:
                    key = "Membership_InvalidQuestion";
                    break;
                case 6:
                    key = "Membership_InvalidAnswer";
                    break;
                case 7:
                    key = "Membership_InvalidEmail";
                    break;
                case 99:
                    key = "Membership_AccountLockOut";
                    break;
                default:
                    key = "Provider_Error";
                    break;
            }

            return key;
        }

        #endregion Métodos Privados

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }
    }
}
