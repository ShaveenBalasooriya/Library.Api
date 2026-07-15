using Library.Api.Application.Interfaces;
using Library.Api.Contracts.Borrowings;
using Library.Api.Filters;

namespace Library.Api.Endpoints;

public static class BorrowingEndpoint
{
    public static void MapBorrowingsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/borrowings").WithTags("Borrowing");

        group.MapGet("/", async (IBorrowingService borrowingService, CancellationToken ct) =>
            Results.Ok(await borrowingService.GetAllAsync(ct)));

        group.MapPost("/", async (BorrowingBookRequest request, IBorrowingService borrowingService, CancellationToken ct) =>
        {
            var borrowing = await borrowingService.BorrowAsync(request, ct);
            return Results.Created($"/api/borrowings/{borrowing.Id}", borrowing);
        }).AddEndpointFilter<ValidationFilter<BorrowingBookRequest>>();

        group.MapPost("/{id:guid}/return", async (Guid id, IBorrowingService borrowingService, CancellationToken ct) =>
            Results.Ok(await borrowingService.ReturnAsync(id, ct)));
    }
}
