﻿namespace CommentsAPI.Models
{
    public class ChangePasswordDTO
    {
        public required string? OldPassword { get; set; }

        public required string? NewPassword { get; set; }
    }
}
