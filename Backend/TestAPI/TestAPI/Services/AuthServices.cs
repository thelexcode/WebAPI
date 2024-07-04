using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TestAPI.Models;
using TestAPI.Repository;
using TestAPI.Request;
using BCrypt.Net;
using Org.BouncyCastle.Crypto.Generators;

namespace TestAPI.Services
{
    public class AuthService
    {
        private readonly UsersRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(UsersRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> AuthenticateUserAsync(string email, string password)
        {
            // Verifica le credenziali dell'utente
            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return null;

            // Genera il token JWT
            var token = GenerateJwtToken(user);
            return token;
        }

        public async Task<bool> RegisterUserAsync(RegisterDto registerRequest)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);

            var user = new Users
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                PasswordHash = hashedPassword,
                RoleId = 1 // Set default RoleId
            };

            return await _userRepository.CreateUserAsync(user);
        }

        private string GenerateJwtToken(Users user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            var tokenLifetime = int.Parse(_configuration["Jwt:TokenLifetime"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddMinutes(tokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
