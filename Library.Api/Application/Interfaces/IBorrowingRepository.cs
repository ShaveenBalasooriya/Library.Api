using Library.Api.Domain.Entities;

namespace Library.Api.Application.Interfaces;

public interface IBorrowingRepository
{
    Task<IReadOnlyList<Borrowing>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Borrowing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Borrowing>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Borrowing>> GetActiveByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default);
    Task<int> GetActiveCountByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);
    Task AddAsync(Borrowing borrowing, CancellationToken cancellationToken = default);
    Task UpdateAsync(Borrowing borrowing, CancellationToken cancellationToken = default);
}
