using System.ComponentModel.DataAnnotations;

namespace CommentsAPI.Models
{
    public class LogInRequestDTO
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
