using Talktoyeat.Domain.Entities;
using Talktoyeat.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Talktoyeat.Core.Repositories
{
    public class BlogRepository : IBlogRepositoy
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<BlogRepository> _logger;

        public BlogRepository(AppDbContext context, ILogger<BlogRepository> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task CreateAsync(Post entity)
        {
            try
            {
                // Ensure Published is explicitly set if you want the post to appear in published lists
                if (!entity.PublishedOn.HasValue)
                    entity.PublishedOn = DateTime.UtcNow;

                // Try to get existing author
                var author = await _dbContext.Authors
                    .Include(a => a.Posts)
                    .FirstOrDefaultAsync(a => a.Id == 1);

                if (author != null)
                {
                    entity.Author = author;
                    author.Posts.Add(entity); // Ensure bidirectional linking
                }
                else
                {
                    author = new Author
                    {
                        Id = 1,
                        Name = "Nahush",
                        Surname = "Karki",
                        Description = "Software Developer",
                        Posts = new List<Post> { entity } // Bidirectional linking
                    };
                    _dbContext.Authors.Add(author);
                }

                // Attach tags properly (preventing duplicates)
                foreach (var tag in entity.Tags.ToList())
                {
                    var existingTag = await _dbContext.Tags
                        .FirstOrDefaultAsync(t => t.Name.ToLower() == tag.Name.ToLower());

                    if (existingTag != null)
                    {
                        entity.Tags.Remove(tag);
                        entity.Tags.Add(existingTag); // Use the tracked tag instead
                    }
                }

                _dbContext.Posts.Add(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating post.");
                throw;
            }
        }


        public async Task<IEnumerable<Post>> FindAsync(Expression<Func<Post, bool>> expression)
        {
            try
            {
                return await _dbContext.Posts
                    .Include(post => post.Tags)
                    .Include(post => post.Images)
                    .Include(post => post.Author)
                    .Where(expression)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Post>> GetAllAsync(bool includeNonPublished = false)
        {
            try
            {
                var postsQuery = _dbContext.Posts
                    .Include(post => post.Tags)
                    .Include(post => post.Images)
                    .Include(post => post.Author)
                    .AsQueryable();

                if (!includeNonPublished)
                {
                    postsQuery = postsQuery.Where(post => post.Published);
                }

                return await postsQuery
                    .OrderByDescending(p => p.PublishedOn)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<Post?> GetByIdAsync(int id, bool includeNonPublished = false)
        {
            try
            {
                var postQuery = _dbContext.Posts
                    .Where(post => post.Id == id)
                    .Include(post => post.Tags)
                    .Include(post => post.Images)
                    .Include(post => post.Author)
                    .AsQueryable();

                if (!includeNonPublished)
                {
                    postQuery = postQuery.Where(post => post.Published);
                }

                var post = await postQuery.FirstOrDefaultAsync();

                if (post != null && post.Published)
                {
                    post.ReadCount += 1;
                    await UpdateAsync(post);
                }

                return post;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task DeleteAsync(Post post)
        {
            _dbContext.Posts.Remove(post);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(Post entity)
        {
            try
            {
                var existingPost = _dbContext.Posts
                   .Include(p => p.Tags)
                   .Include(p => p.Author)
                   .SingleOrDefault(p => p.Id == entity.Id);

                if (existingPost != null)
                {
                    _dbContext.Entry(existingPost).CurrentValues.SetValues(entity);

                    // Remove deleted tags
                    foreach (var existingTag in existingPost.Tags.ToList())
                    {
                        if (!entity.Tags.Any(t => t.Name.ToLower() == existingTag.Name.ToLower()))
                            _dbContext.Tags.Remove(existingTag);
                    }

                    // Add or update tags
                    foreach (var newTag in entity.Tags)
                    {
                        var existingTag = existingPost.Tags
                            .SingleOrDefault(t => t.Name.ToLower() == newTag.Name.ToLower());

                        if (existingTag != null)
                            _dbContext.Entry(existingTag).CurrentValues.SetValues(newTag);
                        else
                            existingPost.Tags.Add(new Tag { Name = newTag.Name });
                    }

                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Post>> GetPopularPostsAsync(int count)
        {
            try
            {
                return await _dbContext.Posts
                    .Where(post => post.Published)
                    .OrderByDescending(p => p.ReadCount)
                    .Include(post => post.Tags)
                    .Include(post => post.Images)
                    .Include(post => post.Author)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Tag>> GettagsAsync()
        {
            try
            {
                return await _dbContext.Tags
                    .GroupBy(t => t.Name.ToLower())
                    .Select(g => g.First())
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Post>> GetPostsByAuthorIdAsync(int authorId)
        {
            try
            {
                return await _dbContext.Posts
                    .Where(p => p.Author != null && p.Author.Id == authorId)
                    .OrderByDescending(p => p.PublishedOn)
                    .Include(post => post.Tags)
                    .Include(post => post.Images)
                    .Include(post => post.Author)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
