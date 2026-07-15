using Library.Api.Contracts.Members;

namespace Library.Api.Application.Interfaces;

public interface IMemberService
{
    Task<IReadOnlyList<MemberResponse>> GetAllAsync(CancellationToken ct = default);
    Task<MemberResponse> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<MemberResponse> RegisterAsync(RegisterMemberRequest request, CancellationToken ct = default);
    Task<MemberResponse> UpdateAsync(Guid id, UpdateMemberRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
