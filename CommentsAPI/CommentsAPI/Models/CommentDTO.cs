using CommentsAPI.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CommentsAPI.Models
{
    public class CommentDTO
    {
        public int? CommentId { get; set; }
        public int? ThreadId { get; set; }
        public int? CreatorId { get; set; }
        public int? ParentId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? LastEdit { get; set; }
        public bool? IsActive { get; set; } = true;

        public UserDTO? Creator { get; set; }
    }
}
