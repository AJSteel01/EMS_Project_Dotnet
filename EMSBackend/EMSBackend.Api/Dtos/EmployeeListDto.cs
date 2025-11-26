namespace EMSBackend.Api.Dtos;

public record class EmployeeListDto
(
    int EmpId,
    string Name,
    string Department,
    decimal Salary
);