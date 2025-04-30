using TaskManagement.Models;

namespace TaskManagement.Services.Interface
{
    public interface IJwtService
    {
        public string GenerateToken(User user);
    }
}
