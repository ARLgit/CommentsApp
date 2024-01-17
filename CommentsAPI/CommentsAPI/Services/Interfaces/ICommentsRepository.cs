using CommentsAPI.Entities;

namespace CommentsAPI.Services.Interfaces
{
    public interface ICommentsRepository
    {
        Task<IEnumerable<Comment>> GetCommentsAsync(int threadId);
        Task<Comment?> GetCommentAsync(int commentId, bool includeReplies);
        Task<bool> CommentExistsAsync(int? commentId);
        Task<bool> CreateCommentAsync(Comment comment);
        Task<bool> UpdateCommentAsync(Comment comment);
        Task<bool> DeleteCommentAsync(Comment comment);
        Task<IEnumerable<Comment>> GetRepliesAsync(int parentCommentId);
        Task<IEnumerable<Comment>> GetCommentsByUserAsync(int creatorId);
    }
}
