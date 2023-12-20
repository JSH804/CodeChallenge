using System;

namespace CodeChallenge.Models
{
    public record ReportingStructure
    {
        public ReportingStructure(Employee employee, int numberOfReports)
        {
            Employee = employee;
            NumberOfReports = numberOfReports;
        }

        public Employee Employee { get; init; }
        public Int32 NumberOfReports { get; init; }
    }
}
