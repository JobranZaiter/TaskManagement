using TaskManagement.Models;

namespace TaskManagement.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        Task<bool> EmailExistsAsync(string email);
        Task SaveChangesAsync();
    }
}
