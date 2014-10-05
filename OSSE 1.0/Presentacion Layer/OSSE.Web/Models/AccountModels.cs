using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;
using Resources;

namespace OSSE.Web.Models
{
    #region Modelos

    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessageResourceName = "Coincidencia", ErrorMessageResourceType = typeof(Usuario))]
    public class ChangePasswordModel
    {
        [Required(ErrorMessageResourceName = "OldPasswordRequerido", ErrorMessageResourceType = typeof(Usuario))]
        [DataType(DataType.Password)]
        [DisplayName("Antigua Contraseña")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceName = "NewPasswordRequerido", ErrorMessageResourceType = typeof(Usuario))]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("Nueva Contraseña")]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceName = "ConfirmPasswordRequerido", ErrorMessageResourceType = typeof(Usuario))]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña Actual")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido.")]
        [DisplayName("Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        [DisplayName("Recordar mi cuenta?")]
        public bool RememberMe { get; set; }

        public string ConexionLocal { get; set; }
    }

    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "La contraseña y la contraseña de confirmación no coinciden.")]
    public class RegisterModel
    {
        [Required]
        [DisplayName("Usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Dirección de correo electronico")]
        public string Email { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirmar contraseña")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetModel
    {

        [Required(ErrorMessage = "La dirección de correo electronico es requerido.")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "El email ingresado, no es válido.")]
        [DisplayName("Escribe tu dirección de correo electrónico")]
        public string Email { get; set; }

    }

    public class AccountModel
    {
        public ResetModel Reset { get; set; }
        public LogOnModel LogOn { get; set; }
    }

    #endregion

    #region Validation

    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Vaya a http://go.microsoft.com/fwlink/?LinkID=177550 para
            // obtener una lista completa de códigos de estado.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "El nombre de usuario ya existe. Escriba otro nombre de usuario.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Ya existe un nombre de usuario para esa dirección de correo electrónico. Especifique otra dirección de correo electrónico.";

                case MembershipCreateStatus.InvalidPassword:
                    return "La contraseña especificada no es válida. Escriba un valor de contraseña válido.";

                case MembershipCreateStatus.InvalidEmail:
                    return "La dirección de correo electrónico especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "La respuesta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "La pregunta de recuperación de la contraseña especificada no es válida. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.InvalidUserName:
                    return "El nombre de usuario especificado no es válido. Compruebe el valor e inténtelo de nuevo.";

                case MembershipCreateStatus.ProviderError:
                    return "El proveedor de autenticación devolvió un error. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";

                case MembershipCreateStatus.UserRejected:
                    return "La solicitud de creación de usuario se ha cancelado. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";

                default:
                    return "Error desconocido. Compruebe los datos especificados e inténtelo de nuevo. Si el problema continúa, póngase en contacto con el administrador del sistema.";
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "'{0}' y '{1}' no coinciden.";
        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(DefaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }
        public string OriginalProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Equals(originalValue, confirmValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "'{0}' debe tener al menos {1} caracteres.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(DefaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            var valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }

    #endregion

}

