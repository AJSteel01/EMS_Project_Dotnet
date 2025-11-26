using EMSBackend.Api.Data;
using EMSBackend.Api.Dtos;
using EMSBackend.Api.Entities;
using EMSBackend.Api.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMSBackend.Api.Endpoints;

public static class EmployeesEndpoints
{
    const string GetEmployeeEndpointName = "GetEmployee";

    public static RouteGroupBuilder MapEmployeesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/employees");


        // GET /employees — Admin Only

        group.MapGet("/", async (
            EMSDbContext db,
            string? search,
            int page = 1,
            int pageSize = 5) =>
        {
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 5;

            IQueryable<Employee> query = db.Employees.Include(e => e.Department);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                query = query.Where(e =>
                    e.Name.ToLower().Contains(search) ||
                    e.Email.ToLower().Contains(search) ||
                    e.Department!.Name.ToLower().Contains(search)
                );
            }

            int totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(e => e.EmpId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => e.ToEmployeeListDto())
                .AsNoTracking()
                .ToListAsync();

            return Results.Ok(new PagedEmployeesDto(items, totalCount));
        })
        .RequireAuthorization("AdminOnly"); // Only Admin can view list



        // GET /employees/{id}
        // Admin → can view anyone
        // Employee → can view ONLY own record

        group.MapGet("/{id}", async (
            int id,
            EMSDbContext db,
            HttpContext context,
            UserManager<ApplicationUser> userManager) =>
        {
            var emp = await db.Employees
                              .Include(e => e.Department)
                              .FirstOrDefaultAsync(e => e.EmpId == id);

            if (emp is null)
                return Results.NotFound();

            var user = await userManager.GetUserAsync(context.User);

            if (user is null)
                return Results.Unauthorized();

            // If user is Employee, then self-access only
            bool isEmployee = await userManager.IsInRoleAsync(user, "Employee");

            if (isEmployee && user.EmployeeId != id)
                return Results.Forbid();

            return Results.Ok(emp.ToEmployeeResponseDto());
        })
        .RequireAuthorization();



        // POST /employees — ADMIN only

        group.MapPost("/", async (EmployeeCreateDto dto, EMSDbContext db) =>
        {
            if (!await db.Departments.AnyAsync(d => d.Id == dto.DepartmentId))
                return Results.BadRequest("Invalid DepartmentId");

            var emp = dto.ToEntity();
            db.Employees.Add(emp);
            await db.SaveChangesAsync();

            return Results.CreatedAtRoute(GetEmployeeEndpointName, new { id = emp.EmpId }, emp.ToEmployeeResponseDto());
        })
        .RequireAuthorization("AdminOnly");

        // PUT /employees/{id}
        // Admin -> can update all fields  
        // Employee -> can ONLY update phone + address  

        group.MapPut("/{id}", async (
            int id,
            EmployeeUpdateDto dto,
            EMSDbContext db,
            HttpContext context,
            UserManager<ApplicationUser> userManager) =>
        {
            var existing = await db.Employees.FindAsync(id);
            if (existing is null)
                return Results.NotFound();

            var user = await userManager.GetUserAsync(context.User);
            if (user is null)
                return Results.Unauthorized();

            bool isAdmin = await userManager.IsInRoleAsync(user, "Admin");
            bool isEmployee = await userManager.IsInRoleAsync(user, "Employee");

            // EMPLOYEE restrict: to Only update their own phone & address
            if (isEmployee)
            {
                if (user.EmployeeId != id)
                    return Results.Forbid();

                // Allow only Address & Phone
                existing.Address = dto.Address;
                existing.Phone = dto.Phone;

                await db.SaveChangesAsync();
                return Results.Ok("Profile updated");
            }

            // ADMIN: full update
            if (!await db.Departments.AnyAsync(d => d.Id == dto.DepartmentId))
                return Results.BadRequest("Invalid DepartmentId");

            db.Entry(existing).CurrentValues.SetValues(dto.ToEntity(id));
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .RequireAuthorization();


        // PUT /employees/self
        // Employee updates phone/address ONLY

        group.MapPut("/self", async (
            EmployeeProfileUpdateDto dto,
            EMSDbContext db,
            HttpContext context,
            UserManager<ApplicationUser> userManager) =>
        {
            var user = await userManager.GetUserAsync(context.User);
            if (user is null || user.EmployeeId is null)
                return Results.Unauthorized();

            var emp = await db.Employees.FirstOrDefaultAsync(e => e.EmpId == user.EmployeeId.Value);

            if (emp is null)
                return Results.NotFound("Employee record not found");

            emp.Phone = dto.Phone;
            emp.Address = dto.Address;

            await db.SaveChangesAsync();
            return Results.Ok("Profile updated");
        })
        .RequireAuthorization("EmployeeOnly");


        // DELETE /employees/{id} — Admin only

        group.MapDelete("/{id}", async (int id, EMSDbContext db) =>
        {
            await db.Employees.Where(e => e.EmpId == id).ExecuteDeleteAsync();
            return Results.NoContent();
        })
        .RequireAuthorization("AdminOnly");

        return group;
    }
}
