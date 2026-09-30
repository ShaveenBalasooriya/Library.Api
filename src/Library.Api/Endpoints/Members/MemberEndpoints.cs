using Application.Abstractions.Authentication;
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

        group.MapGet("me", GetMyProfile).RequireAuthorization(Policies.Member);

        group.MapPut("me", UpdateMyProfile).RequireAuthorization(Policies.Member);

        group.MapGet("{id:guid}", GetMemberById).RequireAuthorization(Policies.Admin);

        group.MapGet("", GetAllMembers).RequireAuthorization(Policies.Admin);

        group.MapDelete("{id:guid}", RemoveMember).RequireAuthorization();
    }

    private static async Task<IResult> GetMyProfile(
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetMyProfileQuery();
        var result = await sender.Send(query, cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemDetails();
    }

    private static async Task<IResult> UpdateMyProfile(
        UpdateMyProfileRequestDto request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new UpdateMyProfileCommand(request.PhoneNumber);
        var result = await sender.Send(command, cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
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
