using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.Books
{
    internal sealed class GetBookQueryHandler : IQueryHandler<GetBookQuery, BookResponse>
    {
        private readonly IBookRepository _bookRepository;

        public GetBookQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Result<BookResponse>> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken);
            if (book is null)
            {
                return Result<BookResponse>.Failure(new Error("Book.NotFound", $"Book with ID '{request.Id}' was not found."));
            }

            var response = new BookResponse(
                book.Id,
                book.Title,
                book.Author,
                book.Isbn.Value,
                book.PublishedYear.Value,
                book.Copies.Total,
                book.Copies.Available);

            return Result<BookResponse>.Success(response);
        }
    }
}
