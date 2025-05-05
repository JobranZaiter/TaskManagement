using Microsoft.AspNetCore.Identity.Data;
using TaskManagement.Models;
using TaskManagement.Repository.Interface;
using TaskManagement.Services.Interface;
using TaskManagement.Models.DTOs;
using TaskManagement.Models.DTOs.Requests;

namespace TaskManagement.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService jwtService;
        private readonly IUserRepository userRepository;
        private readonly IPasswordManager passwordmanager;

        public AuthService(IJwtService _jwtService, IUserRepository _userRepository, IPasswordManager passwordmanager)
        {
            this.userRepository = _userRepository;
            this.jwtService = _jwtService;
            this.passwordmanager = passwordmanager;
        }
        public async Task<(bool Success, string Message)> RegisterAsync(RegisterReq request)
        {
            if (await userRepository.EmailExistsAsync(request.Email))
            {
                return (false, "Email already exists");
            }
            try
            {
                passwordmanager.CreatePasswordHasher(request.Password, out string passwordHash, out string passwordSalt);
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                };
                await userRepository.AddAsync(user);
                await userRepository.SaveChangesAsync();
                return (true, "User registered successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return(false, "Error Occured");
            }

        }

        public async Task<(bool Success, string Message, string Token)> LoginAsync(LoginReq request)
        {

            var user = await userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return (false, "User not found, sign up for a new account", null);
            }
            if (!passwordmanager.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return (false, "Invalid password", null);
            }
            var token = jwtService.GenerateToken(user);
            return (true, "Login successful", token);
        }
    }
}
