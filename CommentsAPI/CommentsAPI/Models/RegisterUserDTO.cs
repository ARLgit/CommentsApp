using System.ComponentModel.DataAnnotations;

namespace CommentsAPI.Models
{
    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "El nuevo Usuario debe poseer un nombre de Usuario.")]
        public required string UserName { get; set; }
        [Required(ErrorMessage = "El nuevo Usuario debe poseer una contraseña.")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "El nuevo Usuario debe poseer un Email.")]
        public required string Email { get; set; }
    }
}
