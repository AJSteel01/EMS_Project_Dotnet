using EMSBackend.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EMSBackend.Api.Data;

public class EMSDbContext(DbContextOptions<EMSDbContext> options)
    : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();

   protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Department>().HasKey(d => d.Id);
    modelBuilder.Entity<Employee>().HasKey(e => e.EmpId);

    modelBuilder.Entity<Department>().HasData(
        new { Id = 1, Name = "Engineering" },
        new { Id = 2, Name = "HR" },
        new { Id = 3, Name = "Finance" },
        new { Id = 4, Name = "Sales" }
    );

    modelBuilder.Entity<Employee>().HasData(
        new
        {
            EmpId = 1,
            Name = "John Doe",
            Email = "john.doe@gmail.com",
            Phone = "9876543210",
            Address = "Mumbai",
            DepartmentId = 1,
            Salary = 60000m,
            DateOfJoining = new DateOnly(2024,01,10)
        }
    );

    modelBuilder.Entity<Employee>()
        .HasIndex(e => e.Email)
        .IsUnique();
}

}
