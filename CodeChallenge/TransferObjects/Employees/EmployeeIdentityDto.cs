using CodeChallenge.Models;
using System;

namespace CodeChallenge.TransferObjects.Employees
{
    public record EmployeeIdentityDto
    {
        public EmployeeIdentityDto(Employee employee)
        {
            EmployeeId = employee.EmployeeId;
            FirstName = employee.FirstName;
            LastName = employee.LastName;
            Position = employee.Position;
            Department = employee.Department;
        }
        public String EmployeeId { get; init; }
        public String FirstName { get; init; }
        public String LastName { get; init; }
        public String Position { get; init; }
        public String Department { get; init; }
    }
}
