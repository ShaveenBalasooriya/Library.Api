using Application.Abstractions.Data;
using Application.Books;
using Application.Borrowings;
using Application.Members;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // "librarydb" matches the Aspire resource name, so Aspire, Docker and local config all resolve the same key.
        var connectionString = configuration.GetConnectionString("librarydb")
            ?? throw new InvalidOperationException("Connection string 'librarydb' is not configured.");

        services.AddDbContext<LibraryDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<LibraryDbContext>());

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IMemberRepository, MemberRepository>();
        services.AddScoped<IBorrowingRepository, BorrowingRepository>();

        services.AddHealthChecks()
            .AddDbContextCheck<LibraryDbContext>(tags: ["ready"]);

        return services;
    }
}
