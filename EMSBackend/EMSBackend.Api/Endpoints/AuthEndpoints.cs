using EMSBackend.Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EMSBackend.Api.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");

        // REGISTER
        group.MapPost("/register", async (
            RegisterRequest request,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            EMSDbContext db) =>
        {
            if (request.Role == "Employee")
            {
                if (request.EmployeeId is null)
                    return Results.BadRequest("EmployeeId is required for Employee role.");

                bool exists = await db.Employees
                    .AnyAsync(e => e.EmpId == request.EmployeeId.Value);

                if (!exists)
                    return Results.BadRequest("Invalid EmployeeId. Employee does not exist.");
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                EmployeeId = request.EmployeeId
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return Results.BadRequest(result.Errors);

            if (!await roleManager.RoleExistsAsync(request.Role))
                await roleManager.CreateAsync(new IdentityRole(request.Role));

            await userManager.AddToRoleAsync(user, request.Role);

            return Results.Ok("User registered successfully");
        });

        // LOGIN
        group.MapPost("/login", async (
    LoginRequest request,
    SignInManager<ApplicationUser> signInManager) =>
{
    var result = await signInManager.PasswordSignInAsync(
        userName: request.Email,   
        password: request.Password,
        isPersistent: true,
        lockoutOnFailure: false);

    return result.Succeeded
        ? Results.Ok("Login successful")
        : Results.Unauthorized();
});

        // LOGOUT
        group.MapPost("/logout", async (SignInManager<ApplicationUser> signInManager) =>
        {
            await signInManager.SignOutAsync();
            return Results.Ok("Logged out");
        });

        // CURRENT USER
        group.MapGet("/me", async (
            UserManager<ApplicationUser> userManager,
            HttpContext context) =>
        {
            var user = await userManager.GetUserAsync(context.User);
            if (user is null)
                return Results.Unauthorized();

            var roles = await userManager.GetRolesAsync(user);

            return Results.Ok(new
            {
                user.FullName,
                user.Email,
                user.EmployeeId,
                Roles = roles
            });
        })
        .RequireAuthorization();

        return app;
    }
}

public record RegisterRequest(string Email, string Password, string FullName, int? EmployeeId, string Role);
public record LoginRequest(string Email, string Password);
