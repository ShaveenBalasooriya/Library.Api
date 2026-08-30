using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Books;
using Domain.Enums;
using Domain.Shared;

namespace Application.Borrowings
{
    internal sealed class ReturnBookCommandHandler : ICommandHandler<ReturnBookCommand>
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReturnBookCommandHandler(
            IBorrowingRepository borrowingRepository,
            IBookRepository bookRepository,
            IUnitOfWork unitOfWork)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        {
            var borrowing = await _borrowingRepository.GetByIdAsync(request.BorrowingId, cancellationToken);
            if (borrowing is null)
            {
                return Result.Failure(new Error("Borrowing.NotFound", $"Borrowing with ID '{request.BorrowingId}' was not found.", ErrorType.NotFound));
            }

            var book = await _bookRepository.GetByIdAsync(borrowing.BookId, cancellationToken);
            if (book is null)
            {
                return Result.Failure(new Error("Book.NotFound", $"Book with ID '{borrowing.BookId}' was not found.", ErrorType.NotFound));
            }

            var returnResult = borrowing.ReturnBook();
            if (returnResult.IsFailure)
            {
                return Result.Failure(returnResult.Error);
            }

            var returnCopyResult = book.ReturnCopy();
            if (returnCopyResult.IsFailure)
            {
                return Result.Failure(returnCopyResult.Error);
            }

            _borrowingRepository.Update(borrowing);
            _bookRepository.Update(book);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
