﻿using System.ComponentModel.DataAnnotations;
using SIGCOMT.DTO.Core;
using SIGCOMT.DTO.GlobalResources;

namespace SIGCOMT.DTO
{
    public class UsuarioDto : EntityDto<int>
    {
        [Required(ErrorMessageResourceName = "UsernameRequerido",  ErrorMessageResourceType = typeof(Usuario) )]
        [StringLength(20, MinimumLength = 4, ErrorMessageResourceName = "UsernameLength", ErrorMessageResourceType = typeof(Usuario))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "PasswordRequerido", ErrorMessageResourceType = typeof(Usuario))]
        [StringLength(20, MinimumLength = 4, ErrorMessageResourceName = "PasswordLength", ErrorMessageResourceType = typeof(Usuario))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "EmailRequerido", ErrorMessageResourceType = typeof(Usuario))]
        //[EmailAddress(ErrorMessageResourceName = "EmailFormato", ErrorMessageResourceType = typeof(Usuario))]
        public string Email { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Idioma { get; set; }
        public int Estado { get; set; }
    }
}