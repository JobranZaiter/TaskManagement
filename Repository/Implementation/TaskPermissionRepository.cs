using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Repository.Interface;

namespace TaskManagement.Repository.Implementation
{
    public class TaskPermissionRepository : ITaskPermissionRepository
    {
        private readonly AppDbContext _context;

        public TaskPermissionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskPermission>> GetByUserAndTaskIdsAsync(int userId, int taskId)
        {
            return await _context.TaskPermissions
                .Include(p => p.Task)
                .Include(p => p.ProjectAssignee)
                .Where(p => p.ProjectAssignee.UserId == userId && p.TaskId == taskId)
                .ToListAsync();
        }
        public async Task<List<TaskPermission>> GetByUserIdAsync(int userId)
        {
            return await _context.TaskPermissions
                .Include(p=>p.Task)
                .Include(p => p.ProjectAssignee)
                .Where(p => p.ProjectAssignee.UserId == userId)
                .ToListAsync();
        }
        public async Task<List<TaskPermission>> GetByTaskIdAsync(int taskId)
        {
            return await _context.TaskPermissions
                .Include(p => p.ProjectAssignee)
                .Where(p => p.TaskId == taskId)
                .ToListAsync();
        }

        public async Task<TaskPermission?> GetByIdAsync(int id)
        {
            return await _context.TaskPermissions
                .Include(p => p.Task)
                .Include(p => p.ProjectAssignee)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(TaskPermission permission)
        {
            await _context.TaskPermissions.AddAsync(permission);
        }

        public async Task DeleteAsync(TaskPermission permission)
        {
            _context.TaskPermissions.Remove(permission);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
