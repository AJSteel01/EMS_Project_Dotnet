using System.ComponentModel.DataAnnotations;

namespace EMSFrontend.Frontend.Models;

public class EmployeeDetails
{
    public int EmpId { get; set; }

    [Required, StringLength(50)]
    public string Name { get; set; } = "";

    [Required, EmailAddress]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Please select a department.")]
    [Range(1, int.MaxValue, ErrorMessage = "Department is required.")]
    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    [Range(10000, 1000000, ErrorMessage = "Salary must be between 10,000 and 1,000,000")]
    public decimal Salary { get; set; }

    [Required]
    public DateOnly DateOfJoining { get; set; }

    [Phone(ErrorMessage = "Invalid phone number format")]
    public string? Phone { get; set; }

    [StringLength(100)]
    public string? Address { get; set; }
}
