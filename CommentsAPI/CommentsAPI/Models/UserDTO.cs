using System.ComponentModel.DataAnnotations;

namespace CommentsAPI.Models
{
    public class UserDTO
    {
        [Required(ErrorMessage = "El Usuario debe poseer un Id.")]
        public required int Id { get; set; }
        [Required(ErrorMessage = "El Usuario debe poseer un nombre de Usuario.")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "El Usuario debe poseer un Email.")]
        public required string Email { get; set; }
    }

    public class UpdateUserDTO
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
    }

    public class RegisterUserDTO
    {
        [Required(ErrorMessage = "El nuevo Usuario debe poseer un nombre de Usuario.")]
        public required string UserName { get; set; }
        [Required(ErrorMessage = "El nuevo Usuario debe poseer una contraseña.")]
        public required string Password { get; set; }
        [Required(ErrorMessage = "El nuevo Usuario debe poseer un Email.")]
        public required string Email { get; set; }
    }

    public class LogInRequestDTO
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }
    }

    public class ChangePasswordDTO
    {
        [Required]
        public required string OldPassword { get; set; }
        [Required]
        public required string NewPassword { get; set; }
    }
}
