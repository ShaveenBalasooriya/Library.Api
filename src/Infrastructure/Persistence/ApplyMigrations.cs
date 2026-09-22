using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public static class ApplyMigrationsExtensions
{
    public static async Task ApplyMigration(this IServiceProvider services, CancellationToken ct = default)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<LibraryDbContext>>();

        logger.LogInformation("Applying database migrations");
        await context.Database.MigrateAsync(ct);
        logger.LogInformation("Database migrations applied");
    }
}
