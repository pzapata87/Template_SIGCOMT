using System.ComponentModel.DataAnnotations;
using SIGCOMT.Resources;
using SIGCOMT.Resources.CustomModelMetadata;

namespace SIGCOMT.DTO
{
    [MetadataConventions(ResourceType = typeof(Usuario))]
    public class LogInDto
    {
        [Required]
        [Display]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display]
        public string Password { get; set; }
    }
}