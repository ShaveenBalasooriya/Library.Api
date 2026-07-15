using Library.Api.Application.Interfaces;
using Library.Api.Domain.Entities;
using Library.Api.Domain.Enums;
using Library.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Infrastructure.Repositories;

public class BorrowingRepository(AppDbContext context) : IBorrowingRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IReadOnlyList<Borrowing>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Borrowings
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Borrowing?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Borrowings
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Borrowing>> GetByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.Borrowings
            .AsNoTracking()
            .Where(b => b.MemberId == memberId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Borrowing>> GetActiveByBookIdAsync(Guid bookId, CancellationToken cancellationToken = default)
    {
        return await _context.Borrowings
            .AsNoTracking()
            .Where(b => b.BookId == bookId && b.Status != BorrowingStatus.Returned)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetActiveCountByMemberIdAsync(Guid memberId, CancellationToken cancellationToken = default)
    {
        return await _context.Borrowings
            .AsNoTracking()
            .CountAsync(b => b.MemberId == memberId && b.Status != BorrowingStatus.Returned, cancellationToken);
    }

    public async Task AddAsync(Borrowing borrowing, CancellationToken cancellationToken = default)
    {
        _context.Borrowings.Add(borrowing);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Borrowing borrowing, CancellationToken cancellationToken = default)
    {
        _context.Borrowings.Update(borrowing);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
