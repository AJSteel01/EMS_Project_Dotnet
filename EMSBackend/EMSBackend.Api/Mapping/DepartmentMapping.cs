using EMSBackend.Api.Dtos;
using EMSBackend.Api.Entities;

namespace EMSBackend.Api.Mapping;

public static class DepartmentMapping
{
    public static DepartmentDto ToDto(this Department dept) => new(dept.Id, dept.Name);
}
