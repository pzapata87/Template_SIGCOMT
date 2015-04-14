using SIGCOMT.DomainValidation.Core;
using SIGCOMT.Resources;

namespace SIGCOMT.DomainValidation.Excepciones
{
    public class UsuarioDuplicadoException : ErrorException
    {
        public UsuarioDuplicadoException() : base(Usuario.UserNameDuplicado)
        {
            Log.InfoFormat("Se intentó agregar un nombre de usuario ya existente en la base de datos...");
        }
    }
}