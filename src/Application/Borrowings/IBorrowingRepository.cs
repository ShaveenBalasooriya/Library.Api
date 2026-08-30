using Domain.Entities;

namespace Application.Borrowings;

public interface IBorrowingRepository
{
    void Add(Borrowing borrowing);

    void Update(Borrowing borrowing);

    Task<Borrowing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Borrowing>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Borrowing>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Borrowing>> GetActiveBorrowingsAsync(Guid memberId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Borrowing>> GetBorrowedAsync(CancellationToken cancellationToken = default);
}
