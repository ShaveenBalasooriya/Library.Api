using Library.Api.Application.Exceptions;
using Library.Api.Application.Interfaces;
using Library.Api.Contracts.Books;
using Library.Api.Domain.Entities;

namespace Library.Api.Application.Services;

public class BookService(IBookRepository bookRepository) : IBookService
{
    public async Task<IReadOnlyList<BookResponse>> GetAllAsync(CancellationToken ct = default)
    {
        var books = await bookRepository.GetAllAsync(ct);
        return [.. books.Select(MapToResponse)];
    }

    public async Task<BookResponse> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var book = await bookRepository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Book with id '{id}' not found.");
        return MapToResponse(book);
    }

    public async Task<BookResponse> CreateAsync(CreateBookRequest request, CancellationToken ct = default)
    {
        var existing = await bookRepository.GetByIsbnAsync(request.Isbn, ct);
        if (existing is not null)
            throw new ConflictException($"A book with ISBN '{request.Isbn}' already exists.");

        Book book;
        try
        {
            book = new Book(request.Title, request.Author, request.Isbn, request.PublishedYear, request.TotalCopies);
        }
        catch (ArgumentException ex)
        {
            throw new BusinessRuleException(ex.Message);
        }

        await bookRepository.AddAsync(book, ct);
        return MapToResponse(book);
    }

    public async Task<BookResponse> UpdateAsync(Guid id, UpdateBookRequest request, CancellationToken ct = default)
    {
        var book = await bookRepository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Book with id '{id}' not found.");

        try
        {
            book.Update(request.Title, request.Author, request.PublishedYear, request.TotalCopies);
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
        {
            throw new BusinessRuleException(ex.Message);
        }

        await bookRepository.UpdateAsync(book, ct);
        return MapToResponse(book);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var book = await bookRepository.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Book with id '{id}' not found.");
        await bookRepository.DeleteAsync(book, ct);
    }

    private static BookResponse MapToResponse(Book book) => new(
        book.Id, book.Title, book.Author, book.Isbn,
        book.PublishedYear, book.TotalCopies, book.AvailableCopies);
}