using Library.Api.Application.Interfaces;
using Library.Api.Contracts.Books;
using Library.Api.Filters;

namespace Library.Api.Endpoints;

public static class BookEndpoint
{
    public static void MapBooksEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/books").WithTags("Books");

        group.MapGet("/", async (IBookService bookService, CancellationToken ct) =>
            Results.Ok(await bookService.GetAllAsync(ct)));

        group.MapGet("/{id:guid}", async (Guid id, IBookService bookService, CancellationToken ct) =>
            Results.Ok(await bookService.GetByIdAsync(id, ct)));

        group.MapPost("/", async (CreateBookRequest request, IBookService bookService, CancellationToken ct) =>
        {
            var book = await bookService.CreateAsync(request, ct);
            return Results.Created($"/api/books/{book.Id}", book);
        }).AddEndpointFilter<ValidationFilter<CreateBookRequest>>();

        group.MapPut("/{id:guid}", async (Guid id, UpdateBookRequest request, IBookService bookService, CancellationToken ct) =>
            Results.Ok(await bookService.UpdateAsync(id, request, ct))).AddEndpointFilter<ValidationFilter<UpdateBookRequest>>();

        group.MapDelete("/{id:guid}", async (Guid id, IBookService bookService, CancellationToken ct) =>
        {
            await bookService.DeleteAsync(id, ct);
            return Results.NoContent();
        });
    }
}
