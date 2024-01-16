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
}
