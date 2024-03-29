﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommentsAPI.Entities
{
    public class Comment
    {
        [Required]
        public required int CommentId { get; set; }

        [Required]
        public required int ThreadId { get; set; }

        [Required]
        public required int? CreatorId { get; set; } = null;

        public int? ParentId { get; set; }

        [Required]
        public required string Content { get; set; }

        [Required]
        public required DateTime CreationDate { get; set; } = DateTime.Now;

        [Required]
        public required DateTime LastEdit { get; set; } = DateTime.Now;

        [Required]
        public required bool IsActive { get; set; } = true;


        [ForeignKey("CreatorId")]
        public virtual ApplicationUser? Creator { get; set; }

        [ForeignKey("ThreadId")]
        public virtual Thread? Thread { get; set; }

        [ForeignKey("ParentId")]
        public virtual Comment? Parent { get; set; }

        public virtual IEnumerable<Comment>? Replies { get; set; }
    }
}
