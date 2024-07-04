using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestAPI.Models;
using TestAPI.Repository;
using TestAPI.Request;

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(EmployeeRepository _employeeRepository) : ControllerBase
    {
      

        // GET: api/employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _employeeRepository.GetEmployeesAsync();
            return Ok(employees);
        }

        // GET: api/employees/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        // POST: api/employees
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(EmployeeDto employeeRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = new Employee
            {
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
                Email = employeeRequest.Email,
                Department = employeeRequest.Department
            };

            var id = await _employeeRepository.CreateEmployeeAsync(employee);
            employee.Id = id;

            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }
        // PUT: api/employees/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, EmployeeDto employeeRequest)
        {
            if (id != employeeRequest.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = new Employee
            {
                Id = id,
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
                Email = employeeRequest.Email,
                Department = employeeRequest.Department
            };

            var success = await _employeeRepository.UpdateEmployeeAsync(employee);
            if (!success)
            {
                return NotFound();
            }

            // Return an object with a message
            return Ok(new { message = "Employee updated successfully." });
        }

        // DELETE: api/employees/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var success = await _employeeRepository.DeleteEmployeeAsync(id);
            if (!success) 
            {
                return NotFound();
            }

            // Return an object with a message
            return Ok(new { message = "Employee deleted successfully." });
        }

    }
}
