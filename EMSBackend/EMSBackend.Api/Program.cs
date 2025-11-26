using EMSBackend.Api.Data;
using EMSBackend.Api.Endpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ------------------ DB Contexts -----------------------
builder.Services.AddDbContext<EMSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EMS")));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthDB")));

// ------------------ Identity -----------------------
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();
// builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
//     .AddCookie(IdentityConstants.ApplicationScheme);

// ------------------ Cookie Paths -----------------------
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/auth/login";
    opt.LogoutPath = "/auth/logout";
    opt.AccessDeniedPath = "/auth/denied";
});

// ------------------ AUTHORIZATION POLICIES -----------------------
builder.Services.AddAuthorization(options =>
{
    // POLICY REQUIRED IN YOUR ENDPOINTS
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));

    // Optional general policy
    options.AddPolicy("EmployeeOnly", policy =>
        policy.RequireRole("Employee"));
});

var app = builder.Build();

// ------------------ Middleware -----------------------
app.UseAuthentication();
app.UseAuthorization();

// ------------------ Endpoints -----------------------
app.MapEmployeesEndpoints();
app.MapDepartmentsEndpoints();
app.MapAuthEndpoints();

// ------------------ MIGRATIONS + SEEDING -----------------------
await app.MigrateDbAsync();
await app.SeedDefaultRolesAsync();
await app.SeedAdminAsync();

// ------------------ Run -----------------------
app.Run();
