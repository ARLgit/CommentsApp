using Microsoft.AspNetCore.Identity;

namespace CommentsAPI.Entities
{
    public class ApplicationUser: IdentityUser<int>
    {
        public virtual IEnumerable<Thread>? Threads { get; set; }
        public virtual IEnumerable<Comment>? Comments { get; set; }
    }
}
