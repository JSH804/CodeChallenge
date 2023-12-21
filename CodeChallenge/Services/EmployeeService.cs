using System;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;
using System.Threading.Tasks;
using CodeChallenge.TransferObjects.Compensations;


namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<Boolean> EmployeeExist(String id)
        {
            Employee employee = await _employeeRepository.GetByEmployeeIdAsync(id);

            return employee != null;
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.AddEmployee(employee);
                await _employeeRepository.SaveAsync();
            }

            return employee;
        }

        public async Task<Employee> GetByIdAsync(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return await _employeeRepository.GetByEmployeeIdAsync(id);
            }

            return null;
        }

        public async Task<Employee> ReplaceAsync(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.RemoveEmployee(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    await _employeeRepository.SaveAsync();

                    _employeeRepository.AddEmployee(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                await _employeeRepository.SaveAsync();
            }

            return newEmployee;
        }

        public async Task<ReportingStructure> GetEmployeeReportingStructureAsync(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                Employee employee = await _employeeRepository.GetByEmployeeIdAsync(id);

                if (employee == null)
                {
                    return null;
                }

                var numberOfEmployees = 0;
                if (employee.DirectReports != null)
                {
                    numberOfEmployees = await GetDirectReportsCountAsync(employee, employee.DirectReports.Count);
                }

                return new ReportingStructure(employee, numberOfEmployees);
            }

            return null;
        }

        //Recursive method so we can get all levels of DirectReports dynamically
        private async Task<int> GetDirectReportsCountAsync(Employee employee, int numberOfEmployees)
        {
            foreach (Employee emp in employee.DirectReports)
            {
                var childEmployee = await _employeeRepository.GetByEmployeeIdAsync(emp.EmployeeId);

                if (childEmployee.DirectReports != null && childEmployee.DirectReports.Count != 0)
                {
                    numberOfEmployees = await GetDirectReportsCountAsync(childEmployee, numberOfEmployees + childEmployee.DirectReports.Count);
                }
            }

            return numberOfEmployees;
        }

        public async Task<Compensation> CreateCompensation(Compensation compensation)
        {
            _employeeRepository.AddCompensation(compensation);
            await _employeeRepository.SaveAsync();

            return compensation;
        }

        public async Task<Compensation> GetEmployeeCompensation(String id)
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                return await _employeeRepository.GetEmployeeCompensationAsync(id);
            }

            return null;
        }
    }
}
