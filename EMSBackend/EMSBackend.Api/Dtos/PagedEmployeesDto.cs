namespace EMSBackend.Api.Dtos;

public record PagedEmployeesDto(
    List<EmployeeListDto> Items,
    int TotalCount
);
