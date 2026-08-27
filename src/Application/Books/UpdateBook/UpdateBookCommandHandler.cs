using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Shared;
using Domain.ValueObjects;

namespace Application.Books
{
    internal sealed class UpdateBookCommandHandler : ICommandHandler<UpdateBookCommand>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken);
            if (book is null)
            {
                return Result.Failure(new Error("Book.NotFound", $"Book with ID '{request.Id}' was not found."));
            }

            var isbnResult = Isbn.Create(request.Isbn);
            if (isbnResult.IsFailure)
            {
                return Result.Failure(isbnResult.Error);
            }

            var publishedYearResult = PublishedYear.Create(request.PublishedYear);
            if (publishedYearResult.IsFailure)
            {
                return Result.Failure(publishedYearResult.Error);
            }

            if (isbnResult.Value != book.Isbn)
            {
                bool isUnique = await _bookRepository.IsIsbnUniqueAsync(isbnResult.Value, cancellationToken);
                if (!isUnique)
                {
                    return Result.Failure(new Error("Book.DuplicateIsbn", $"A book with ISBN '{request.Isbn}' already exists."));
                }
            }

            var updateResult = book.Update(request.Title, request.Author, isbnResult.Value, publishedYearResult.Value, request.TotalCopies);
            if (updateResult.IsFailure)
            {
                return Result.Failure(updateResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
