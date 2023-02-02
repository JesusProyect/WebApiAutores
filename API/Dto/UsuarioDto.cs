using System.ComponentModel.DataAnnotations;

namespace API.Dto
{
    public class UsuarioBaseDto
    {
    }

    public class EditarAdminDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
    public class AutenticacionRespuestaDto : UsuarioBaseDto
    {
        public string? Token { get; set; }
        public DateTime Expiracion { get; set; }

    }

    public class CredencialesDto : UsuarioBaseDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Formato de Email no válido")]
        public string? Email { get; set; }

        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$", ErrorMessage ="La Clave debe Contener 1 Mayúscula, 1 Minúscula, 1 Número, 1 Caracter especial y entre 6 y 10 Caracteres")]
        public string? Password { get; set; } 

    }
}
