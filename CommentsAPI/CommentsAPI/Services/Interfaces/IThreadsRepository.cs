using CommentsAPI.Entities;

namespace CommentsAPI.Services.Interfaces
{
    public interface IThreadsRepository
    {
        Task<IEnumerable<Entities.Thread>> GetThreadsAsync();
        Task<Entities.Thread?> GetThreadAsync(int threadId, bool includeComments);
        Task<bool> ThreadExistsAsync(int threadId);
        Task<bool> CreateThreadAsync(Entities.Thread thread);
        Task<bool> UpdateThreadAsync(Entities.Thread thread);
        Task<bool> DeleteThreadAsync(Entities.Thread thread);
        Task<IEnumerable<Entities.Thread>> GetRepliesAsync(int threadId);
    }
}
