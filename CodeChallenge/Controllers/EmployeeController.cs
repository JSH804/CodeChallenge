using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;
using System.Threading.Tasks;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            await _employeeService.CreateAsync(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public async Task<IActionResult> GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = await _employeeService.GetByIdAsync(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = await _employeeService.GetByIdAsync(id);
            if (existingEmployee == null)
                return NotFound();

            await _employeeService.ReplaceAsync(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }

        [HttpGet("{id}/reportingstructure")]
        public async Task<IActionResult> GetEmployeeReportingStructure(String id)
        {
            _logger.LogDebug($"Received employee reporting structure request for employee {id}");

            try
            {
                var reportingStructure = await _employeeService.GetEmployeeReportingStructureAsync(id);

                if (reportingStructure == null)
                    return NotFound();

                return Ok(reportingStructure);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Error getting reporting structure for employee {id}");
                return new StatusCodeResult(500);
            }

        }
    }
}
