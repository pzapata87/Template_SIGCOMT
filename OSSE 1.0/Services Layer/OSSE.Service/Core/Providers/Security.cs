using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;

namespace OSSE.Service.Core.Providers
{
    public static class Security
    {
        #region Variables

        private static string _sHashAlgorithm;

        #endregion Variables

        #region propiedades

        public static CustomIdentity CurrentUserIdentity
        {
            get
            {
                var custonIdentity = (CustomIdentity)CurrentUser.Identity;
                return custonIdentity;
            }
        }

        /// <summary>
        /// If the current user is authenticated, returns the current MembershipUser. If not, returns null. This is just a shortcut to Membership.GetUser().
        /// </summary>
        public static MembershipUser CurrentMembershipUser
        {
            get
            {
                return Membership.GetUser();
            }
        }

        /// <summary>
        /// Gets the current user for the current HttpContext.
        /// </summary>
        /// <remarks>
        /// This should always return HttpContext.Current.User. That value and Thread.CurrentPrincipal can't be
        /// guaranteed to always be the same value, as they can be set independently from one another. Looking
        /// through the .Net source, the System.Web.Security.Roles class also returns the HttpContext's User.
        /// </remarks>
        public static IPrincipal CurrentUser
        {
            get
            {
                return HttpContext.Current.User;
            }
        }

        /// <summary>
        /// Gets whether the current user is logged in.
        /// </summary>
        public static bool IsAuthenticated
        {
            get
            {
                return CurrentUser.Identity.IsAuthenticated;
            }
        }

        /// <summary>
        /// Gets whether the currently logged in user is in the administrator role.
        /// </summary>
        public static bool IsAdministrator
        {
            get
            {
                var user = CurrentUser.Identity.Name;
                const string rol = "Administrador";

                if (string.IsNullOrEmpty(user))
                    return false;

                return (IsAuthenticated && CurrentUser.IsInRole(rol));
            }
        }

        #endregion propiedades

        #region Métodos de seguridad

        private static string GetHashAlgorithm()
        {
            if (_sHashAlgorithm != null)
            {
                return _sHashAlgorithm;
            }
            var hashAlgorithmType = Membership.HashAlgorithmType;

            var algorithm = HashAlgorithm.Create(hashAlgorithmType);
            if (algorithm == null)
            {
                throw new ConfigurationErrorsException("Invalid_hash_algorithm_type");
            }
            return _sHashAlgorithm = hashAlgorithmType;
        }

        #region Encriptar

        /// <summary>
        /// Método para encriptar un texto plano usando el algoritmo (Rijndael).
        /// Este es el mas simple posible, muchos de los datos necesarios los
        /// definimos como constantes.
        /// </summary>
        /// <param name="textoQueEncriptaremos">texto a encriptar</param>
        /// <returns>Texto encriptado</returns>
        public static string Encriptar(string textoQueEncriptaremos)
        {
            if (string.IsNullOrWhiteSpace(textoQueEncriptaremos))
                return string.Empty;
            return Encriptar(textoQueEncriptaremos, "pass75dc@avz10", "s@lAvz", GetHashAlgorithm(), 1, "@1B2c3D4e5F6g7H8", 64);
        }

        /// <summary>
        /// Método para encriptar un texto plano usando el algoritmo (Rijndael)
        /// </summary>
        /// <returns>Texto encriptado</returns>
        public static string Encriptar(string textoQueEncriptaremos,
          string passBase, string saltValue, string hashAlgorithm,
          int passwordIterations, string initVector, int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(textoQueEncriptaremos);
            var password = new PasswordDeriveBytes(passBase, saltValueBytes, hashAlgorithm, passwordIterations);
// ReSharper disable once CSharpWarnings::CS0618
            byte[] keyBytes = password.GetBytes(keySize / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }

        #endregion Encriptar

        #region Desencriptar

        /// <summary>
        /// Método para desencriptar un texto encriptado.
        /// </summary>
        /// <returns>Texto desencriptado</returns>
        public static string Desencriptar(string textoEncriptado)
        {
            if (string.IsNullOrWhiteSpace(textoEncriptado))
                return string.Empty;
            return Desencriptar(textoEncriptado, "pass75dc@avz10", "s@lAvz", GetHashAlgorithm(), 1, "@1B2c3D4e5F6g7H8", 64);
        }

        /// <summary>
        /// Método para desencriptar un texto encriptado (Rijndael)
        /// </summary>
        /// <returns>Texto desencriptado</returns>
        public static string Desencriptar(string textoEncriptado, string passBase,
          string saltValue, string hashAlgorithm, int passwordIterations,
          string initVector, int keySize)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
            byte[] cipherTextBytes = Convert.FromBase64String(textoEncriptado);
            var password = new PasswordDeriveBytes(passBase, saltValueBytes, hashAlgorithm, passwordIterations);
// ReSharper disable once CSharpWarnings::CS0618
            byte[] keyBytes = password.GetBytes(keySize / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC };
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
            return plainText;
        }

        #endregion Desencriptar

        #endregion Métodos de seguridad

        #region Métodos de validaciones

        public static bool GetBooleanValue(NameValueCollection config, string valueName, bool defaultValue)
        {
            string sValue = config[valueName];
            if (sValue == null)
            {
                return defaultValue;
            }

            bool result;
            if (bool.TryParse(sValue, out result))
            {
                return result;
            }
            throw new ProviderException("Value_must_be_boolean, " + valueName);
        }

        public static int GetIntValue(NameValueCollection config, string valueName, int defaultValue, bool zeroAllowed, int maxValueAllowed)
        {
            string sValue = config[valueName];

            if (sValue == null)
            {
                return defaultValue;
            }

            int iValue;
            if (!Int32.TryParse(sValue, out iValue))
            {
                if (zeroAllowed)
                {
                    throw new ProviderException("Value_must_be_non_negative_integer, " + valueName);
                }

                throw new ProviderException("Value_must_be_positive_integer, " + valueName);
            }

            if (zeroAllowed && iValue < 0)
            {
                throw new ProviderException("Value_must_be_non_negative_integer, " + valueName);
            }

            if (!zeroAllowed && iValue <= 0)
            {
                throw new ProviderException("Value_must_be_positive_integer, " + valueName);
            }

            if (maxValueAllowed > 0 && iValue > maxValueAllowed)
            {
                throw new ProviderException("Value_too_big, " + valueName + ", max: " + maxValueAllowed.ToString(CultureInfo.InvariantCulture));
            }

            return iValue;
        }

        public static void CheckParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
        {
            if (param == null)
            {
                if (checkForNull)
                {
                    throw new ArgumentNullException(paramName);
                }

                return;
            }

            param = param.Trim();
            if (checkIfEmpty && param.Length < 1)
            {
                throw new ArgumentException("Parameter_can_not_be_empty, paramName", paramName);
            }

            if (maxSize > 0 && param.Length > maxSize)
            {
                throw new ArgumentException(paramName + " Parameter_too_long, max:" + maxSize.ToString(CultureInfo.InvariantCulture), paramName);
            }

            if (checkForCommas && param.Contains(","))
            {
                throw new ArgumentException("Parameter_can_not_contain_comma, " + paramName, paramName);
            }
        }

        public static void CheckArrayParameter(ref string[] param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
        {
            if (param == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (param.Length < 1)
            {
                throw new ArgumentException("Parameter_array_empty, paramName, " + paramName);
            }

            var values = new Hashtable(param.Length);
            for (int i = param.Length - 1; i >= 0; i--)
            {
                CheckParameter(ref param[i], checkForNull, checkIfEmpty, checkForCommas, maxSize,
                    paramName + "[ " + i.ToString(CultureInfo.InvariantCulture) + " ]");
                if (values.Contains(param[i]))
                {
                    throw new ArgumentException("Parameter_duplicate_array_element, " + paramName + ", " + paramName);
                }
                values.Add(param[i], param[i]);
            }
        }

        #endregion Métodos de validaciones
    }
}
