using CommentsAPI.Data;
using CommentsAPI.Entities;
using CommentsAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CommentsAPI.Services
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly CommentsDbContext _dbContext;
        public CommentsRepository(CommentsDbContext commentsDbContext)
        {
            _dbContext = commentsDbContext;
        }

        public async Task<bool> CommentExistsAsync(int? commentId)
        {
            try
            {
                if (commentId == null) { return false; }
                bool response = await _dbContext.Comments.AnyAsync(c => c.CommentId == commentId);
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> CreateCommentAsync(Comment comment)
        {
            try
            {
                if (_dbContext.Threads.Where(t => t.ThreadId == comment.ThreadId).Any())
                {
                    await _dbContext.Comments.AddAsync(comment);
                    return (await _dbContext.SaveChangesAsync() > 0);
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteCommentAsync(Comment comment)
        {
            try
            {
                if (comment != null)
                {
                    _dbContext.Comments.Remove(comment);
                    return (await _dbContext.SaveChangesAsync() > 0);
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateCommentAsync(Comment comment)
        {
            try
            {
                if (comment != null)
                {
                    _dbContext.Comments.Update(comment); 
                    return (await _dbContext.SaveChangesAsync() > 0);
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Comment?> GetCommentAsync(int commentId, bool includeReplies)
        {
            try
            {
                if (includeReplies == true)
                {
                    return await _dbContext.Comments.Include(r => r.Replies).FirstOrDefaultAsync(c => c.CommentId == commentId);
                }
                else
                {
                    return await _dbContext.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync(int threadId)
        {
            try
            {
                return await _dbContext.Comments.Where(c => c.ThreadId == threadId).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Comment>> GetRepliesAsync(int parentCommentId)
        {
            try
            {
                return await _dbContext.Comments.Where(c => c.ParentId == parentCommentId).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Comment>> GetCommentsByUserAsync(int creatorId)
        {
            try
            {
                return await _dbContext.Comments.Where(c => c.CreatorId == creatorId).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
