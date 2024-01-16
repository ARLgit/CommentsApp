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
        public DateTime? LastEdit { get; set; }

        public virtual UserDTO? Creator { get; set; } = null;
        public virtual IEnumerable<CommentDTO>? Comments { get; set; } = Enumerable.Empty<CommentDTO>();
    }
}
