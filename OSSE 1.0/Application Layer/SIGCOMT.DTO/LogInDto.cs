using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SIGCOMT.DTO
{
    public class LogInDto
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
}