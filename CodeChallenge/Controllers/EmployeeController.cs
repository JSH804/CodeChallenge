using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;
using System.Threading.Tasks;
using CodeChallenge.TransferObjects.Compensations;
using System.Collections.Generic;
using System.Net;
using System.ComponentModel.DataAnnotations;

//Normally I would use something like Automapper
//Also, I would lean towards using a global error handler instead of the try catches I added below
//I've found the Result pattern to be interesting lately when creating the response type for api's
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
        public async Task<IActionResult> ReplaceEmployee(String id, [FromBody] Employee newEmployee)
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

        [HttpPost("{id}/compensation")]
        public async Task<ActionResult> CreateEmployeeCompensation(String id, [FromBody] CreateCompensationsDto createCompensationsDto)
        {
            _logger.LogDebug("Received request to create an employee compensation", new[] { createCompensationsDto });
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!await _employeeService.EmployeeExist(id))
                {
                    return NotFound("The employee does not exist");
                }

                Compensation compensation = new Compensation()
                {
                    Salary = (Decimal)createCompensationsDto.Salary,
                    EffectiveDate = (DateTime)createCompensationsDto.EffectiveDate,
                    EmployeeId = id
                };

                await _employeeService.CreateCompensation(compensation);

                CompensationDto compensationDto = new CompensationDto(compensation);

                return Ok(compensationDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Error while create compensation for employee {id}");
                return new StatusCodeResult(500);
            }
        }

        [HttpGet("{id}/compensation")]
        public async Task<ActionResult> GetEmployeeCompensation(String id)
        {
            _logger.LogDebug($"Received request to get compensation for employee {id}");

            try
            {
                if (!await _employeeService.EmployeeExist(id))
                {
                    return NotFound("The employee does not exist");
                }
                Compensation compensation = await _employeeService.GetEmployeeCompensation(id);

                if (compensation == null)
                {
                    return NotFound("The employee does not have an associated compensation");
                }

                return Ok(new CompensationDto(compensation));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Error while retrieving compensation for employee {id}");
                return new StatusCodeResult(500);
            }
        }
    }
}
