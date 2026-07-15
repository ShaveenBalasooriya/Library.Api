using Library.Api.Application.Exceptions;
using Library.Api.Application.Interfaces;
using Library.Api.Contracts.Members;
using Library.Api.Domain.Entities;

namespace Library.Api.Application.Services;

public class MemberService(IMemberRepository memberRepository) : IMemberService
{
    public async Task<IReadOnlyList<MemberResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var members = await memberRepository.GetAllAsync(ct);
        return [.. members.Select(MapToResponse)];
    }

    public async Task<MemberResponse> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var member = await memberRepository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Member with id '{id}' not found.");
        return MapToResponse(member);
    }

    public async Task<MemberResponse> RegisterAsync(RegisterMemberRequest request, CancellationToken ct = default)
    {
        var existing = await memberRepository.GetByEmailAsync(request.Email, ct);
        if (existing is not null)
            throw new ConflictException($"A member with email '{request.Email}' already exists.");

        var member = new Member(request.FullName, request.Email, request.PhoneNumber);
        await memberRepository.AddAsync(member, ct);
        return MapToResponse(member);
    }

    public async Task<MemberResponse> UpdateAsync(Guid id, UpdateMemberRequest request, CancellationToken ct = default)
    {
        var member = await memberRepository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Member with id '{id}' not found.");

        member.Update(request.FullName, request.PhoneNumber);
        if (request.IsActive) member.Activate(); else member.Deactivate();

        await memberRepository.UpdateAsync(member, ct);
        return MapToResponse(member);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var member = await memberRepository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Member with id '{id}' not found.");
        await memberRepository.DeleteAsync(member, ct);
    }

    private static MemberResponse MapToResponse(Member member) => new(
        member.Id, member.FullName, member.Email, member.PhoneNumber,
        member.RegisteredDate, member.IsActive);
}
