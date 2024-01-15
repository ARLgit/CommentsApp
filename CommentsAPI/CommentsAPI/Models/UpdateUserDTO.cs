﻿using System.ComponentModel.DataAnnotations;

namespace CommentsAPI.Models
{
    public class UpdateUserDTO
    {
        [Required(ErrorMessage = "El Usuario debe poseer un nombre de Usuario.")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "El Usuario debe poseer un Email.")]
        public required string Email { get; set; }

        public required string? OldPassword { get; set; }

        public required string? NewPassword { get; set; }
    }
}
