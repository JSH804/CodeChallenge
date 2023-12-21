using CodeChallenge.Models;
using CodeChallenge.TransferObjects.Compensations;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetByEmployeeIdAsync(String id);
        Employee AddEmployee(Employee employee);
        Employee RemoveEmployee(Employee employee);

        Compensation AddCompensation(Compensation compensation);
        Task<Compensation> GetEmployeeCompensationAsync(string id);
        Task SaveAsync();
    }
}