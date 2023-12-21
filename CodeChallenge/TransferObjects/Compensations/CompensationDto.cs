using CodeChallenge.Models;
using CodeChallenge.TransferObjects.Employees;
using System;

namespace CodeChallenge.TransferObjects.Compensations
{
    public record CompensationDto
    {
        public CompensationDto(Compensation compensation)
        {
            CompensationId = compensation.CompensationId;
            Salary = compensation.Salary;
            EffectiveDate = compensation.EffectiveDate;

            Employee = new EmployeeIdentityDto(compensation.Employee);
        }
        public String CompensationId { get; init; }
        public Decimal Salary { get; init; }
        public DateTime EffectiveDate { get; init; }

        public EmployeeIdentityDto Employee { get; init; }
    }
}
