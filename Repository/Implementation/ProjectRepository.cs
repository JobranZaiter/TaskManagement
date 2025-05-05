using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Repository.Interface;
namespace TaskManagement.Repository.Implementation
{

    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.ProjectAssignees)
                .ThenInclude(pa => pa.User)
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Project>> GetProjectsByAssigneeId(int id)
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.ProjectAssignees)
                .Include(p => p.Tasks)
                .Where(p => p.ProjectAssignees.Any(pa => pa.UserId == id))
                .ToListAsync();
        }

        public async Task<List<Project>> GetAllByUserIdAsync(int userId)
        {
            return await _context.Projects
                .Include(p => p.ProjectAssignees)
                .Include(p => p.Tasks)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Owner)
                .Include(p => p.ProjectAssignees)
                .Include(p => p.Tasks)
                .ToListAsync();
        }

        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }

        public async Task DeleteAsync(Project project)
        {
            _context.Projects.Remove(project);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Projects.AnyAsync(p => p.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
