using CommentsAPI.Data;
using CommentsAPI.Services.Interfaces;

namespace CommentsAPI.Services
{
    public class ThreadsRepository : IThreadsRepository
    {
        private readonly CommentsDbContext _dbContext;
        public ThreadsRepository(CommentsDbContext commentsDbContext)
        {
            _dbContext = commentsDbContext;
        }

        public Task<bool> CreateThreadAsync(Entities.Thread thread)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteThreadAsync(Entities.Thread thread)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateThreadAsync(Entities.Thread thread)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ThreadExistsAsync(int threadId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Entities.Thread>> GetRepliesAsync(int threadId)
        {
            throw new NotImplementedException();
        }

        public Task<Entities.Thread?> GetThreadAsync(int threadId, bool includeComments)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Entities.Thread>> GetThreadsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
