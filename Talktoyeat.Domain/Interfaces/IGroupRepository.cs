using Talktoyeat.Domain.Entities;

namespace Talktoyeat.Domain.Interfaces
{
    public interface IGroupRepository
    {
        Task<Group> GetByIdAsync(int id);
        Task<IEnumerable<Group>> GetAllAsync();
        Task CreateAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(int id);
    }
}