using Library.Api.Application.Interfaces;
using Library.Api.Contracts.Members;
using Library.Api.Filters;

namespace Library.Api.Endpoints;

public static class MemberEndpoint
{
    public static void MapMembersEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/members").WithTags("Member");

        group.MapGet("/", async (IMemberService memberService, CancellationToken ct) =>
            Results.Ok(await memberService.GetAllAsync(ct)));

        group.MapGet("/{id:guid}", async (Guid id, IMemberService memberService, CancellationToken ct) =>
            Results.Ok(await memberService.GetByIdAsync(id, ct)));

        group.MapPost("/", async (RegisterMemberRequest request, IMemberService memberService, CancellationToken ct) =>
        {
            var member = await memberService.RegisterAsync(request, ct);
            return Results.Created($"/api/members/{member.Id}", member);
        }).AddEndpointFilter<ValidationFilter<RegisterMemberRequest>>();

        group.MapPut("/{id:guid}", async (Guid id, UpdateMemberRequest request, IMemberService memberService, CancellationToken ct) =>
            Results.Ok(await memberService.UpdateAsync(id, request, ct))).AddEndpointFilter<ValidationFilter<UpdateMemberRequest>>();

        group.MapDelete("/{id:guid}", async (Guid id, IMemberService memberService, CancellationToken ct) =>
        {
            await memberService.DeleteAsync(id, ct);
            return Results.NoContent();
        });

        group.MapGet("/{memberId:guid}/borrowings", async (Guid memberId, IBorrowingService borrowingService, CancellationToken ct) =>
           Results.Ok(await borrowingService.GetByMemberIdAsync(memberId, ct)));
    }
}
