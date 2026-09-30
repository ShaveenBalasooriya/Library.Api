using Domain.Entities;
using Domain.Shared;

namespace Application.Members;

public interface ICurrentMemberProvider
{
    Task<Result<Member>> GetOrCreateCurrentMemberAsync(CancellationToken cancellationToken = default);
}
