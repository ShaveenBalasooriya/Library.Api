using Application.Borrowings;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BorrowingRepository(LibraryDbContext dbContext) : IBorrowingRepository
{
    public void Add(Borrowing borrowing)
    {
        dbContext.Borrowings.Add(borrowing);
    }

    public async Task<IReadOnlyList<Borrowing>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Borrowings.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Borrowing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Borrowings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Borrowing>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Borrowings
            .AsNoTracking()
            .Where(b => b.MemberId == memberId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Borrowing>> GetActiveBorrowingsAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Borrowings
            .AsNoTracking()
            .Where(b => b.MemberId == memberId && (b.Status == BorrowingStatus.Borrowed || b.Status == BorrowingStatus.Overdue))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Borrowing>> GetBorrowedAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Borrowings
            .Where(b => b.Status == BorrowingStatus.Borrowed)
            .ToListAsync(cancellationToken);
    }

    public void Update(Borrowing borrowing)
    {
        dbContext.Borrowings.Update(borrowing);
    }
}
