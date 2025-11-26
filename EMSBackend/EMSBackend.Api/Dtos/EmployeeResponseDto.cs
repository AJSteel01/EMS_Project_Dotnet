namespace EMSBackend.Api.Dtos;

public record class EmployeeResponseDto(
    int EmpId,
    string Name,
    string Email,
    int DepartmentId,
    string DepartmentName,
    decimal Salary,
    DateOnly DateOfJoining,
    string? Phone,
    string? Address
);
