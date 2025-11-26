using EMSBackend.Api.Mapping;
using EMSBackend.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace EMSBackend.Api.Endpoints;

public static class DepartmentsEndpoints
{
    public static RouteGroupBuilder MapDepartmentsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("departments");

        group.MapGet("/", async (EMSDbContext dbContext) =>
            await dbContext.Departments
                    .Select(dept => dept.ToDto())
                    .AsNoTracking()
                    .ToListAsync()
        );

        return group;
    }
}
