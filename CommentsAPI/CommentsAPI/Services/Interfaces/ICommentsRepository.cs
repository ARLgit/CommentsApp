using CommentsAPI.Entities;

namespace CommentsAPI.Services.Interfaces
{
    public interface ICommentsRepository
    {
        Task<IEnumerable<Comment>> GetCommentsAsync(int threadId);
        Task<Comment?> GetCommentAsync(int commentId, bool includeReplies);
        Task<bool> CommentExistsAsync(int commentId);
        Task<bool> CreateCommentAsync(Comment comment);
        Task<bool> EditCommentAsync(Comment commentId);
        Task<bool> DeleteCommentAsync(int commentId);
        Task<IEnumerable<Comment>> GetRepliesAsync(int parentCommentId);
    }
}
