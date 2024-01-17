using CommentsAPI.Entities;
using CommentsAPI.Models;

namespace CommentsAPI.Services.Interfaces
{
    public interface IThreadsRepository
    {
        Task<(IEnumerable<Entities.Thread>, PaginationMetadata?)> GetThreadsAsync(int CurrentPage, int PageSize, string? searchQuery);
        Task<Entities.Thread?> GetThreadAsync(int threadId, bool includeComments);
        Task<bool> ThreadExistsAsync(int? threadId);
        Task<bool> CreateThreadAsync(Entities.Thread thread);
        Task<bool> UpdateThreadAsync(Entities.Thread thread);
        Task<bool> DeleteThreadAsync(Entities.Thread thread);
        Task<IEnumerable<Entities.Thread>> GetThreadsByUserAsync(int creatorId);

    }
}
