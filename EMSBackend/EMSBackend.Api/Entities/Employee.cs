using System;

namespace EMSBackend.Api.Entities
{
    public class Employee
    {
        public int EmpId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public decimal Salary { get; set; }
        public DateOnly DateOfJoining { get; set; }

        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}
