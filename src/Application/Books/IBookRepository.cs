using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Books;

public interface IBookRepository
{
    void Add(Book book);

    void Update(Book book);

    void Remove(Book book);

    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<bool> IsIsbnUniqueAsync(Isbn isbn, CancellationToken cancellationToken = default);
}
