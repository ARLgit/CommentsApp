using CommentsAPI.Data;
using CommentsAPI.Entities;
using CommentsAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommentsAPI.Services
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly CommentsDbContext _dbContext;
        public CommentsRepository(CommentsDbContext commentsDbContext)
        {
            _dbContext = commentsDbContext;
        }

        public async Task<bool> CommentExistsAsync(int commentId)
        {
            try
            {
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
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            try
            {
                await _dbContext.Comments.AddAsync(comment);
                var response = await _dbContext.SaveChangesAsync();
                if (response > 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            try
            {
                var comment = await _dbContext.Comments.FirstOrDefaultAsync(c => c.CommentId == commentId);
                _dbContext.Comments.Remove(comment);
                var response = await _dbContext.SaveChangesAsync();
                if (response > 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
            return false;
        }

        public async Task<bool> EditCommentAsync(Comment comment)
        {
            if (comment == null) throw new ArgumentNullException(nameof(comment));
            try
            {
                _dbContext.Comments.Update(comment);
                var response = await _dbContext.SaveChangesAsync();
                if (response > 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
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
    }
}
