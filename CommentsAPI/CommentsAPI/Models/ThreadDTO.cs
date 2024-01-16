using CommentsAPI.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommentsAPI.Models
{
    public class ThreadDTO
    {
        public int? ThreadId { get; set; }
        public int? CreatorId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime LastEdit { get; set; } = DateTime.Now;

        public virtual UserDTO? Creator { get; set; } = null;
        public virtual IEnumerable<CommentDTO>? Comments { get; set; } = Enumerable.Empty<CommentDTO>();
    }

    public class PostThreadDTO
    {
        [Required]
        public int CreatorId { get; set; } = default(int);
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }

    public class UpdateThreadDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
