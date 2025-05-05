using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Repository.Interface;

namespace TaskManagement.Repository.Implementation
{
    public class ProjectAssigneeRepository : IProjectAssigneeRepository
    {
        private readonly AppDbContext _context;

        public ProjectAssigneeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProjectAssignee?> GetByProjectAndUserIdAsync(int projectId, int userId)
        {
            return await _context.ProjectAssignees
                .Include(pa => pa.User)
                .Include(pa => pa.Project)
                .FirstOrDefaultAsync(pa => pa.ProjectId == projectId && pa.UserId == userId);
        }

        public async Task<ProjectAssignee?> GetByIdAsync(int id)
        {
            return await _context.ProjectAssignees
                .Include(pa => pa.User)
                .Include(pa => pa.Project)
                .FirstOrDefaultAsync(pa => pa.Id == id);
        }

        public async Task<List<ProjectAssignee>> GetAllAsync()
        {
            return await _context.ProjectAssignees
                .Include(pa => pa.User)
                .Include(pa => pa.Project)
                .ToListAsync();
        }

        public async Task AddAsync(ProjectAssignee assignee)
        {
            await _context.ProjectAssignees.AddAsync(assignee);
        }

        public async Task DeleteAsync(ProjectAssignee assignee)
        {
            _context.ProjectAssignees.Remove(assignee);
            await Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
