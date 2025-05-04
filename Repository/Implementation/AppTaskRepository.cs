using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Repository.Interface;

namespace TaskManagement.Repository.Implementation
{
    public class AppTaskRepository : IAppTaskRepository
    {
        private readonly AppDbContext _context;

        public AppTaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppTask>> GetByUserIdAsync(int userId)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.SubTasks)
                .Include(t => t.Comments)
                .Where(t => t.Project.ProjectAssignees.Any(t=> t.UserId == userId))
                .ToListAsync();
        }

        public async Task<AppTask?> GetByIdAsync(int id)
        {
            return await _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.Assignee)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(AppTask task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public async Task DeleteAsync(AppTask task)
        {
            _context.Tasks.Remove(task);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
