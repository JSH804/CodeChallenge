using CodeChallenge.Models;
using CodeChallenge.TransferObjects.Compensations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
    public interface IEmployeeService
    {
        Task<Boolean> EmployeeExist(String id);
        Task<Employee> GetByIdAsync(String id);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee> ReplaceAsync(Employee originalEmployee, Employee newEmployee);
        Task<ReportingStructure> GetEmployeeReportingStructureAsync(string id);

        Task<Compensation> CreateCompensation(Compensation compensation);
        Task<Compensation> GetEmployeeCompensation(String Id);
    }
}
