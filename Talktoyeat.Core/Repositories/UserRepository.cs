using Talktoyeat.Domain.Entities;
using Talktoyeat.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Talktoyeat.Core.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly AppDbContext _dbContext;

        public async Task<Author> GetCurrentUserAsync()
        {
            try
            {
                // Replace with actual logic to retrieve the current user
                var currentUser = await _dbContext.Authors.FirstOrDefaultAsync();
                return currentUser!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task FollowAsync(int followerId, int followeeId)
        {
            try
            {
                // Replace with actual logic to follow a user
                var follower = await _dbContext.Authors.FindAsync(followerId);
                var followee = await _dbContext.Authors.FindAsync(followeeId);

                if (follower != null && followee != null)
                {
                    // Add follow logic here
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task UnfollowAsync(int followerId, int followeeId)
        {
            try
            {
                // Replace with actual logic to unfollow a user
                var follower = await _dbContext.Authors.FindAsync(followerId);
                var followee = await _dbContext.Authors.FindAsync(followeeId);

                if (follower != null && followee != null)
                {
                    // Add unfollow logic here
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(AppDbContext dbContext,ILogger<UserRepository> logger )
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task CreateAsync(Author author)
        {
            try
            {
                _dbContext.Authors.Add(author);
                return _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Author> FindAsync(Expression<Func<Author, bool>> expression)
        {
            try
            {
                var author = await _dbContext.Authors
                                  .Where(expression)
                                  .FirstOrDefaultAsync();
                return author!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
