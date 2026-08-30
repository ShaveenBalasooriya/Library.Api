using Application.Members;
using Carter;
using Library.Api.Extensions;
using MediatR;

namespace Library.Api.Endpoints.Members;

public sealed class MemberEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/members").WithTags("Members");

        group.MapPost("", AddMember);

        group.MapGet("{id:guid}", GetMemberById);

        group.MapGet("", GetAllMembers);

        group.MapPut("{id:guid}", UpdateMember);

        group.MapDelete("{id:guid}", RemoveMember);
    }

    private static async Task<IResult> AddMember(
        AddMemberRequestDto request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new AddMemberCommand(
            request.FullName,
            request.Email,
            request.PhoneNumber);

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/members/{result.Value}", result.Value)
            : result.ToProblemDetails();
    }

    private static async Task<IResult> GetMemberById(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetMemberQuery(id);
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    private static async Task<IResult> GetAllMembers(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetAllMembersQuery();
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    private static async Task<IResult> UpdateMember(
        Guid id,
        UpdateMemberRequestDto request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UpdateMemberCommand(
            id,
            request.FullName,
            request.PhoneNumber);

        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemDetails();
    }

    private static async Task<IResult> RemoveMember(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new RemoveMemberCommand(id);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemDetails();
    }
}
