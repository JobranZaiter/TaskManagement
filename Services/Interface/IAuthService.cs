using TaskManagement.Models.DTOs;
using TaskManagement.Models.DTOs.Requests;

namespace TaskManagement.Services.Interface
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(RegisterReq request);
        Task<(bool Success, string Message, string Token)> LoginAsync(LoginReq request);
    }

}
