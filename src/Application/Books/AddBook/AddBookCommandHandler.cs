using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Books
{
    internal sealed class AddBookCommandHandler : ICommandHandler<AddBookCommand, Guid>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var isbnResult = Isbn.Create(request.Isbn);
            if (isbnResult.IsFailure)
            {
                return Result<Guid>.Failure(isbnResult.Error);
            }

            var publishedYearResult = PublishedYear.Create(request.PublishedYear);
            if (publishedYearResult.IsFailure)
            {
                return Result<Guid>.Failure(publishedYearResult.Error);
            }

            var copiesResult = BookCopies.Create(request.TotalCopies);
            if (copiesResult.IsFailure)
            {
                return Result<Guid>.Failure(copiesResult.Error);
            }

            bool isUnique = await _bookRepository.IsIsbnUniqueAsync(isbnResult.Value, cancellationToken);
            if (!isUnique)
            {
                return Result<Guid>.Failure(new Error("Book.DuplicateIsbn", $"A book with ISBN '{request.Isbn}' already exists."));
            }

            var bookResult = Book.Create(request.Title, request.Author, isbnResult.Value, publishedYearResult.Value, copiesResult.Value);
            if (bookResult.IsFailure)
            {
                return Result<Guid>.Failure(bookResult.Error);
            }

            var book = bookResult.Value;

            _bookRepository.Add(book);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(book.Id);
        }
    }
}
