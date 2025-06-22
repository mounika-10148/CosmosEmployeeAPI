using Microsoft.AspNetCore.Mvc;
using CosmosEmployeeAPI.Models;
using CosmosEmployeeAPI.Services;

namespace CosmosEmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly CosmosDbService _cosmosDbService;

        public EmployeeController(CosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet("{department}")]
        public async Task<IActionResult> GetEmployees(string department)
        {
            var employees = await _cosmosDbService.GetEmployeesAsync(department);
            return Ok(employees);
        }

        [HttpGet("{department}/{id}")]
        public async Task<IActionResult> GetEmployee(string department, string id)
        {
            var employee = await _cosmosDbService.GetEmployeeAsync(id, department);
            if (employee == null) return NotFound();
            return Ok(employee);
        }

      
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Employee employee)
    {
        if (employee == null)
            return BadRequest("Employee cannot be null");

        if (string.IsNullOrEmpty(employee.Department))
            return BadRequest("Department (partition key) is required");

        var createdEmployee = await _cosmosDbService.AddEmployeeAsync(employee);
        return CreatedAtAction(nameof(Post), new { id = createdEmployee.Id }, createdEmployee);
    }



        [HttpPut("{department}/{id}")]
        public async Task<IActionResult> UpdateEmployee(string department, string id, [FromBody] Employee employee)
        {
            if (id != employee.Id) return BadRequest("ID mismatch");
            await _cosmosDbService.UpdateEmployeeAsync(id, employee);
            return NoContent();
        }

        [HttpDelete("{department}/{id}")]
        public async Task<IActionResult> DeleteEmployee(string department, string id)
        {
            await _cosmosDbService.DeleteEmployeeAsync(id, department);
            return NoContent();
        }
    }
    }
