using Library.Api.Application.Interfaces;
using Library.Api.Domain.Entities;
using Library.Api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Infrastructure.Repositories;

public class MemberRepository(AppDbContext context) : IMemberRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IReadOnlyList<Member>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Members
            .FirstOrDefaultAsync(m => m.Email == email, cancellationToken);
    }

    public async Task AddAsync(Member member, CancellationToken cancellationToken = default)
    {
        _context.Members.Add(member);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Member member, CancellationToken cancellationToken = default)
    {
        _context.Members.Update(member);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Member member, CancellationToken cancellationToken = default)
    {
        _context.Members.Remove(member);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
