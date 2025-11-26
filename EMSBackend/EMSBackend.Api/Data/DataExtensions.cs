using Microsoft.EntityFrameworkCore;

namespace EMSBackend.Api.Data;

public static class DataExtensions
{
    public static async Task MigrateDbAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EMSDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}
