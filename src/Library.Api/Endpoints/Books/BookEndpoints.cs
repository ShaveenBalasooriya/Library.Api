using Application.Abstractions.Authentication;
using Application.Books;
using Carter;
using Library.Api.Extensions;
using MediatR;

namespace Library.Api.Endpoints.Books;

public sealed class BookEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/books").WithTags("Books");

        group.MapPost("", AddBook).RequireAuthorization(Policies.Admin);

        group.MapGet("{id:guid}", GetBookById).RequireAuthorization();

        group.MapGet("", GetAllBooks).RequireAuthorization();

        group.MapPut("{id:guid}", UpdateBook).RequireAuthorization(Policies.Admin);

        group.MapDelete("{id:guid}", RemoveBook).RequireAuthorization(Policies.Admin);
    }

    private static async Task<IResult> AddBook(
        BookRequestDto request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new AddBookCommand(
            request.Title,
            request.Author,
            request.Isbn,
            request.PublishedYear,
            request.TotalCopies);

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/books/{result.Value}", result.Value)
            : result.ToProblemDetails();
    }

    private static async Task<IResult> GetBookById(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetBookQuery(id);
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    private static async Task<IResult> GetAllBooks(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAllBooksQuery();
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    private static async Task<IResult> UpdateBook(
        Guid id,
        BookRequestDto request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UpdateBookCommand(
            id,
            request.Title,
            request.Author,
            request.Isbn,
            request.PublishedYear,
            request.TotalCopies);

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemDetails();
    }

    private static async Task<IResult> RemoveBook(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new RemoveBookCommand(id);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemDetails();
    }
}
