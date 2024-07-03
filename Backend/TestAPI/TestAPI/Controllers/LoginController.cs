using Microsoft.AspNetCore.Mvc;
using TestAPI.Models;
using TestAPI.Services;
using System.Threading.Tasks;
using TestAPI.Request; 

namespace TestAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController (AuthService _authService): ControllerBase
    {

        // GET: api/login
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await _authService.AuthenticateUserAsync(loginRequest.Email, loginRequest.PasswordHash);
            if (token == null)
                return Unauthorized();

            return Ok(new { Token = token });
        }
    }
}
