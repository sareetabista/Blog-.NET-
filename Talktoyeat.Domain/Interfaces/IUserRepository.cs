using Talktoyeat.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Talktoyeat.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Author> GetCurrentUserAsync();
        Task<Author> FindAsync(Expression<Func<Author, bool>> predicate);
        Task CreateAsync(Author user);
        Task FollowAsync(int currentUserId, int targetUserId);
        Task UnfollowAsync(int currentUserId, int targetUserId);
    }
}
