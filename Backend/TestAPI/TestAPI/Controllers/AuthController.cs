using Microsoft.AspNetCore.Mvc;
using TestAPI.Models;
using TestAPI.Services;
using System.Threading.Tasks;
using TestAPI.Request;

namespace TestAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // POST: api/register
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterDto registerRequest)
        {
            var result = await _authService.RegisterUserAsync(registerRequest);

            if (!result)
                return BadRequest("Registration failed");

            return Ok(new { message = "Registration successfully." });
        }

        // POST: api/login
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginDto loginRequest)
        {
            var token = await _authService.AuthenticateUserAsync(loginRequest.Email, loginRequest.PasswordHash);
            if (token == null)
                return Unauthorized();

            return Ok(new { Token = token });
        }
    }
}
