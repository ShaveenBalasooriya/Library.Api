using Library.Api.Contracts.Books;

namespace Library.Api.Application.Interfaces;

public interface IBookService
{
    Task<IReadOnlyList<BookResponse>> GetAllAsync(CancellationToken ct = default);
    Task<BookResponse> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<BookResponse> CreateAsync(CreateBookRequest request, CancellationToken ct = default);
    Task<BookResponse> UpdateAsync(Guid id, UpdateBookRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
