using EMSBackend.Api.Dtos;
using EMSBackend.Api.Entities;

namespace EMSBackend.Api.Mapping;

public static class EmployeeMapping
{
    // Create DTO -> Entity
    public static Employee ToEntity(this EmployeeCreateDto dto)
{
    return new Employee
    {
        Name = dto.Name,
        Email = dto.Email,
        Salary = dto.Salary,
        DateOfJoining = dto.DateOfJoining,
        Phone = dto.Phone,
        Address = dto.Address,
        DepartmentId = dto.DepartmentId
    };
}

    // Update DTO -> Entity (for CurrentValues.SetValues usage)
    public static Employee ToEntity(this EmployeeUpdateDto dto, int id)
{
    return new Employee
    {
        EmpId = id,
        Name = dto.Name,
        Email = dto.Email,
        Salary = dto.Salary,
        DateOfJoining = dto.DateOfJoining,
        Phone = dto.Phone,
        Address = dto.Address,
        DepartmentId = dto.DepartmentId
    };
}

    // Entity -> list DTO
    public static EmployeeListDto ToEmployeeListDto(this Employee emp) =>
        new(emp.EmpId, emp.Name, emp.Department?.Name ?? string.Empty, emp.Salary);

    // Entity -> response DTO (detailed)
    public static EmployeeResponseDto ToEmployeeResponseDto(this Employee emp) =>
    new(emp.EmpId, emp.Name, emp.Email, emp.DepartmentId, emp.Department?.Name ?? string.Empty,
        emp.Salary, emp.DateOfJoining, emp.Phone, emp.Address);

}
