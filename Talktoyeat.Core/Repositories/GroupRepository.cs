// using Microsoft.EntityFrameworkCore;
// using Talktoyeat.Domain.Entities;
// using Talktoyeat.Domain.Interfaces;

// namespace Talktoyeat.Core.Repositories
// {
//     public class GroupRepository : IGroupRepository
//     {
//         private readonly AppDbContext _context;

//         public GroupRepository(AppDbContext context)
//         {
//             _context = context;
//         }

//         public async Task<Group> GetByIdAsync(int id) => await _context.Groups.FindAsync(id);

//         public async Task<IEnumerable<Group>> GetAllAsync()
//         {
//             try
//             {
//                 // Fetching all groups
//                 var groups = await _context.Groups.ToListAsync();
//                 return groups;
//             }
//             catch (Exception ex)
//             {
//                 // Log the exception (or handle it accordingly)
//                 // Example: _logger.LogError("An error occurred while fetching groups: ", ex);
//                 throw new Exception("An error occurred while fetching groups.", ex);
//             }
//         }

//         public async Task CreateAsync(Group group)
//         {
//             _context.Groups.Add(group);
//             await _context.SaveChangesAsync();
//         }

//         public async Task UpdateAsync(Group group)
//         {
//             _context.Groups.Update(group);
//             await _context.SaveChangesAsync();
//         }

//         public async Task DeleteAsync(int id)
//         {
//             var group = await GetByIdAsync(id);
//             if (group != null)
//             {
//                 _context.Groups.Remove(group);
//                 await _context.SaveChangesAsync();
//             }
//         }

//         public Group GetGroupById(int groupId)
//         {
//             var group = _context.Groups
//                 .Include(g => g.Members)
//                 .FirstOrDefault(g => g.Id == groupId);

//             if (group == null)
//             {
//                 throw new Exception("Group not found");
//             }

//             return group;
//         }
          
//     }
// }