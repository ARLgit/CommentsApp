using CommentsAPI.Data;
using CommentsAPI.Models;
using CommentsAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace CommentsAPI.Services
{
    public class ThreadsRepository : IThreadsRepository
    {
        private readonly CommentsDbContext _dbContext;
        public ThreadsRepository(CommentsDbContext commentsDbContext)
        {
            _dbContext = commentsDbContext;
        }

        public async Task<bool> ThreadExistsAsync(int? threadId)
        {
            try
            {
                if (threadId == null) { return false; }
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
                    return await _dbContext.Threads
                        .Include(t => t.Creator)
                        .Include(t => t.Comments)
                        .ThenInclude(c => c.Creator)
                        .FirstOrDefaultAsync(t => t.ThreadId == threadId);
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

        public async Task<(IEnumerable<Entities.Thread>, PaginationMetadata?)> GetThreadsAsync(int CurrentPage = 0, int PageSize = 10, string? searchQuery = null)
        {
            try
            {
                var threads = await _dbContext.Threads.Include(t => t.Creator).ToListAsync();
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    searchQuery = searchQuery.Trim();
                    threads = threads.Where(a => a.Title.ToLower().Contains(searchQuery.ToLower())
                        || (a.Content != null && a.Content.Contains(searchQuery))).ToList();
                }
                if (CurrentPage > 0 && PageSize > 0)
                {
                    var totalItemCount = threads.Count();

                    var paginationMetadata = new PaginationMetadata(
                        totalItemCount, PageSize, CurrentPage, searchQuery);

                    var threadsToReturn = threads
                    .Skip(PageSize * (CurrentPage - 1))
                    .Take(PageSize).ToList();
                    return (threadsToReturn, paginationMetadata);
                }
                return (threads, null);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Entities.Thread>> GetThreadsByUserAsync(int creatorId)
        {
            try
            {
                return await _dbContext.Threads.Where(t => t.CreatorId == creatorId).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
