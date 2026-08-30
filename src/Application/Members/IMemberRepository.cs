using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Members;

public interface IMemberRepository
{
    void Add(Member member);

    void Update(Member member);

    void Remove(Member member);

    Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Member>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);
}
