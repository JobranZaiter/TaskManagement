using Microsoft.AspNetCore.Mvc;
using TaskManagement.Services.Interface;
using TaskManagement.Models.DTOs;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService _authService) 
        { 
            this.authService = _authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterReq request)
        {
            var result = await authService.RegisterAsync(request);
            if (result.Success)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReq request)
        {
            var result = await authService.LoginAsync(request);
            if (result.Success)
            {
                return Ok(new { result.Token });
            }
            return BadRequest(result.Message);
        }

    }
}
