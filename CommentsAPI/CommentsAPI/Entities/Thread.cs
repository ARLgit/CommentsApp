using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CommentsAPI.Entities
{
    public class Thread
    {
        [Required]
        public required int ThreadId { get; set; }

        [Required]
        public required int CreatorId { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Content { get; set; }

        [Required]
        public required DateTime CreationDate { get; set; } = DateTime.Now;

        [Required]
        public required DateTime? LastEdit { get; set; } = DateTime.Now;

        [ForeignKey("CreatorId")]
        public virtual ApplicationUser? Creator { get; set; }

        public virtual IEnumerable<Comment>? Comments { get; set; }
    }
}
