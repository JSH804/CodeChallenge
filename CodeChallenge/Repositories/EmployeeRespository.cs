using System;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;
using CodeChallenge.TransferObjects.Compensations;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee AddEmployee(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public async Task<Employee> GetByEmployeeIdAsync(string id)
        {
            return await _employeeContext.Employees
                .Include(e => e.DirectReports)
                .SingleOrDefaultAsync(e => e.EmployeeId == id);
        }


        public Employee RemoveEmployee(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        public Compensation AddCompensation(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            _employeeContext.Compensations.Add(compensation);

            return compensation;
        }

        //Was leaning towards passing an object with query options for further filtering here
        public async Task<Compensation> GetEmployeeCompensationAsync(String id)
        {
            return await _employeeContext.Compensations
                .Include(c => c.Employee)
                .Where(c => c.EmployeeId == id)
                    .OrderByDescending(c => c.EffectiveDate)
                    .FirstOrDefaultAsync();
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}
