using Application.Borrowings;
using Carter;
using Library.Api.Extensions;
using MediatR;

namespace Library.Api.Endpoints.Borrowings;

public sealed class BorrowingEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/borrowings").WithTags("Borrowings");

        group.MapPost("", BorrowBook);

        group.MapPost("{id:guid}/return", ReturnBook);

        group.MapGet("", GetAllBorrowings);

        group.MapGet("member/{memberId:guid}", GetMemberBorrowings);

        group.MapPost("mark-overdue", MarkOverdueBorrowings);
    }

    private static async Task<IResult> BorrowBook(
        BorrowBookRequestDto request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new BorrowBookCommand(request.BookId, request.MemberId);

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/borrowings/{result.Value}", result.Value)
            : result.ToProblemDetails();
    }

    private static async Task<IResult> ReturnBook(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new ReturnBookCommand(id);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemDetails();
    }

    private static async Task<IResult> GetAllBorrowings(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAllBorrowingsQuery();
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    private static async Task<IResult> GetMemberBorrowings(
        Guid memberId,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetMemberBorrowingsQuery(memberId);
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    private static async Task<IResult> MarkOverdueBorrowings(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new MarkOverdueBorrowingsCommand();
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }
}
