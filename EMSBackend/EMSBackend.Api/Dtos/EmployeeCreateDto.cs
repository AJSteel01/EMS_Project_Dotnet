using System.ComponentModel.DataAnnotations;

namespace EMSBackend.Api.Dtos;

public record class EmployeeCreateDto(
    string Name,
    string Email,
    int DepartmentId,
    decimal Salary,
    DateOnly DateOfJoining,
    string? Phone,
    string? Address
);