using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.Books
{
    internal sealed class GetAllBooksQueryHandler : IQueryHandler<GetAllBooksQuery, IReadOnlyList<BookResponse>>
    {
        private readonly IBookRepository _bookRepository;

        public GetAllBooksQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Result<IReadOnlyList<BookResponse>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _bookRepository.GetAllAsync(cancellationToken);

            var response = books
                .Select(book => new BookResponse(
                    book.Id,
                    book.Title,
                    book.Author,
                    book.Isbn.Value,
                    book.PublishedYear.Value,
                    book.Copies.Total,
                    book.Copies.Available))
                .ToList();

            return Result<IReadOnlyList<BookResponse>>.Success(response);
        }
    }
}
