using CommentsAPI.Data;
using CommentsAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommentsAPI.Services
{
    public class ThreadsRepository : IThreadsRepository
    {
        private readonly CommentsDbContext _dbContext;
        public ThreadsRepository(CommentsDbContext commentsDbContext)
        {
            _dbContext = commentsDbContext;
        }

        public async Task<bool> ThreadExistsAsync(int threadId)
        {
            try
            {
                bool response = await _dbContext.Threads.AnyAsync(t => t.ThreadId == threadId);
                return response;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> CreateThreadAsync(Entities.Thread thread)
        {
            try
            {
                if (thread != null)
                {
                    await _dbContext.Threads.AddAsync(thread);
                    return (await _dbContext.SaveChangesAsync() > 0);
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteThreadAsync(Entities.Thread thread)
        {
            try
            {
                if (thread != null)
                {
                    _dbContext.Threads.Remove(thread);
                    return (await _dbContext.SaveChangesAsync() > 0);
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateThreadAsync(Entities.Thread thread)
        {
            try
            {
                if (thread != null)
                {
                    _dbContext.Threads.Update(thread);
                    return (await _dbContext.SaveChangesAsync() > 0);
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Entities.Thread?> GetThreadAsync(int threadId, bool includeComments)
        {
            try
            {
                if (includeComments == true)
                {
                    return await _dbContext.Threads.Include(t => t.Comments).FirstOrDefaultAsync(t => t.ThreadId == threadId);
                }
                else
                {
                    return await _dbContext.Threads.FirstOrDefaultAsync(t => t.ThreadId == threadId);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Entities.Thread>> GetThreadsAsync()
        {
            try
            {
                return await _dbContext.Threads.ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
