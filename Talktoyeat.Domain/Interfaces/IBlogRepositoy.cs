using Talktoyeat.Domain.Entities;
using System.Linq.Expressions;

namespace Talktoyeat.Domain.Interfaces
{
    public interface IBlogRepositoy
    {
        Task CreateAsync(Post entity);
        Task DeleteAsync(Post entity);
        Task<IEnumerable<Post>> FindAsync(Expression<Func<Post, bool>> expression);
        Task<IEnumerable<Post>> GetAllAsync(bool includeNonPublished = false);
        Task<Post?> GetByIdAsync(int id, bool includeNonPublished = false);
        Task UpdateAsync(Post entity);
        Task<IEnumerable<Post>> GetPopularPostsAsync(int count);
        Task<IEnumerable<Tag>> GettagsAsync();
        Task<IEnumerable<Post>> GetPostsByAuthorIdAsync(int authorId);
    }
}