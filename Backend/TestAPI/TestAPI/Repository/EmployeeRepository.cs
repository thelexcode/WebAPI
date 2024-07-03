using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using TestAPI.Data; 
using TestAPI.Models;

namespace TestAPI.Repository
{
    public class EmployeeRepository
    {
        private readonly DapperContext _context;

        public EmployeeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            using IDbConnection connection = _context.CreateConnection();
            connection.Open();

            string query = "SELECT Id, FirstName, LastName, Email, Department FROM Employee";
            var employees = await connection.QueryAsync<Employee>(query);
            return employees;
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            using IDbConnection connection = _context.CreateConnection();
            connection.Open();

            string query = "SELECT Id, FirstName, LastName, Email, Department FROM Employee WHERE Id = @Id";
            var employee = await connection.QueryFirstOrDefaultAsync<Employee>(query, new { Id = id });
            return employee;
        }

        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            using IDbConnection connection = _context.CreateConnection();
            connection.Open();

            string query = @"
                INSERT INTO Employee (FirstName, LastName, Email, Department)
                VALUES (@FirstName, @LastName, @Email, @Department);
                SELECT LAST_INSERT_ID();
            ";
            var id = await connection.ExecuteScalarAsync<int>(query, employee);
            return id;
        }

        public async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            using IDbConnection connection = _context.CreateConnection();
            connection.Open();

            string query = @"
                UPDATE Employee
                SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Department = @Department
                WHERE Id = @Id
            ";
            var rowsAffected = await connection.ExecuteAsync(query, employee);
            return rowsAffected > 0;
        }


        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            using IDbConnection connection = _context.CreateConnection();
            connection.Open();

            string query = "DELETE FROM Employee WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(query, new { Id = id });
            return rowsAffected > 0;
        }



    }
}
