using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OSSE.Common
{
    public class Security
    {
        private static string _sHashAlgorithm;

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

        private static string GetHashAlgorithm()
        {
            if (_sHashAlgorithm != null)
            {
                return _sHashAlgorithm;
            }

            var hashAlgorithmType = ConfigurationManager.AppSettings["HashAlgorithmType"];

            var algorithm = HashAlgorithm.Create(hashAlgorithmType);
            if (algorithm == null)
            {
                throw new ConfigurationErrorsException("Invalid_hash_algorithm_type");
            }
            return _sHashAlgorithm = hashAlgorithmType;
        }
    }
}