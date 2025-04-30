using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Models;
using TaskManagement.Repository.Interface;

namespace TaskManagement.Repository.Implementation
{

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .Include(u => u.ProjectAssignments)
                .Include(u => u.ProjectOwnerships)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.ProjectAssignments)
                .Include(u => u.ProjectOwnerships)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .Include(u => u.ProjectAssignments)
                .Include(u => u.ProjectOwnerships)
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
