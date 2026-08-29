using Application.Books;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BookRepository(LibraryDbContext dbContext) : IBookRepository
{
    public void Add(Book book)
    {
        dbContext.Books.Add(book);
    }

    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Books.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Books.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public async Task<bool> IsIsbnUniqueAsync(Isbn isbn, CancellationToken cancellationToken = default)
    {
        return !await dbContext.Books.AnyAsync(b => b.Isbn.Value == isbn.Value, cancellationToken);
    }

    public void Update(Book book)
    {
        dbContext.Books.Update(book);
    }

    public void Remove(Book book)
    {
        dbContext.Books.Remove(book);
    }
}
