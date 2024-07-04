using System.Data;
using TestAPI.Data;
using TestAPI.Models;
using Dapper;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace TestAPI.Repository
{
    public class UsersRepository
    {
        private readonly DapperContext _context;

        public UsersRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Users>> GetUsersAsync()
        {
            using IDbConnection connection = _context.CreateConnection();
            connection.Open();

            string query = @"
                SELECT u.Id, u.FirstName, u.LastName, u.Email, u.PasswordHash, r.Description AS RoleDescription
                FROM Users u
                JOIN Roles r ON u.RoleId = r.Id
            ";

            var users = await connection.QueryAsync<Users>(query);
            return users;
        }

        public async Task<Users> GetUserByIdAsync(int id)
        {
            using IDbConnection connection = _context.CreateConnection();
            connection.Open();
            var user = await connection.QuerySingleOrDefaultAsync<Users>("SELECT Id, FirstName, LastName, Email, RoleId FROM Users WHERE Id = @Id", new { Id = id });
            return user;
        }

        public async Task<Users> GetUserByEmailAndPasswordAsync(string email, string passwordHash)
        {
            using IDbConnection connection = _context.CreateConnection();
            connection.Open();

            string query = @"
                SELECT u.Id, u.FirstName, u.LastName, u.Email, u.RoleId
                FROM Users u
                WHERE u.Email = @Email AND u.PasswordHash = @PasswordHash
            ";

            var user = await connection.QuerySingleOrDefaultAsync<Users>(query, new { Email = email, PasswordHash = passwordHash });
            return user;
        }

        public async Task<bool> CreateUserAsync(Users user)
        {
            try
            {
                using IDbConnection connection = _context.CreateConnection();
                connection.Open();

                string query = @"
                    INSERT INTO Users (FirstName, LastName, Email, PasswordHash, RoleId)
                    VALUES (@FirstName, @LastName, @Email, @PasswordHash, @RoleId)
                ";

                var result = await connection.ExecuteAsync(query, new
                {
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.PasswordHash,
                    user.RoleId // Added RoleId parameter
                });

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UsersRepository.CreateUserAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<Users> GetUserByEmailAsync(string email)
        {
            using IDbConnection connection = _context.CreateConnection();
            connection.Open();

            string query = @"
                SELECT Id, FirstName, LastName, Email, PasswordHash, RoleId
                FROM Users
                WHERE Email = @Email
            ";

            var user = await connection.QuerySingleOrDefaultAsync<Users>(query, new { Email = email });
            return user;
        }


        public string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
