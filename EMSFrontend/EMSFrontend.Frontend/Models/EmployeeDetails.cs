namespace EMSFrontend.Frontend.Models;

public class EmployeeDetails
{
    public int EmpId { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string? Phone { get; set; }
    public string? Address { get; set; }

      public int DepartmentId { get; set; }  // For selection
    public string? DepartmentName { get; set; } 

    public decimal Salary { get; set; }
    public DateOnly DateOfJoining { get; set; }
}

