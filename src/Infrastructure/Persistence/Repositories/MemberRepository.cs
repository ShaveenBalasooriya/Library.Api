using Application.Members;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class MemberRepository(LibraryDbContext dbContext) : IMemberRepository
{
    public void Add(Member member)
    {
        dbContext.Members.Add(member);
    }

    public async Task<IReadOnlyList<Member>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Members.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Members.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
    {
        return !await dbContext.Members.AnyAsync(m => m.Email.Value == email.Value, cancellationToken);
    }

    public void Update(Member member)
    {
        dbContext.Members.Update(member);
    }

    public void Remove(Member member)
    {
        dbContext.Members.Remove(member);
    }
}
