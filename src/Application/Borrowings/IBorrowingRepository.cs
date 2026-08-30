using Domain.Entities;

namespace Application.Borrowings;

public interface IBorrowingRepository
{
    void Add(Borrowing borrowing);

    void Update(Borrowing borrowing);

    Task<Borrowing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Borrowing>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Borrowing>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the outstanding (not yet returned) borrowings for a member, i.e. Status is Borrowed or Overdue.
    /// </summary>
    Task<IReadOnlyList<Borrowing>> GetActiveBorrowingsAsync(Guid memberId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns every borrowing currently Status == Borrowed, across all members.
    /// Used by the overdue-sweep job as its set of candidates to re-evaluate against their due date;
    /// borrowings already Overdue or Returned are irrelevant to that sweep.
    /// </summary>
    Task<IReadOnlyList<Borrowing>> GetBorrowedAsync(CancellationToken cancellationToken = default);
}
