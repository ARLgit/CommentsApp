using CommentsAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace CommentsAPI.Data
{
    public class CommentsDbContext : IdentityDbContext<ApplicationUser,IdentityRole<int>,int>
    {
        public CommentsDbContext(DbContextOptions<CommentsDbContext> options) : base(options)
        {
        }

        public DbSet<Entities.Thread> Threads { get; set; }
        public DbSet<Comment> Comments { get; set; }
}
}
