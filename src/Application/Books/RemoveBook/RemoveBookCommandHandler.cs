using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.Books
{
    internal sealed class RemoveBookCommandHandler : ICommandHandler<RemoveBookCommand>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveBookCommandHandler(IBookRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(RemoveBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken);
            if (book is null)
            {
                return Result.Failure(new Error("Book.NotFound", $"Book with ID '{request.Id}' was not found."));
            }

            if (book.Copies.Borrowed > 0)
            {
                return Result.Failure(new Error("Book.HasActiveBorrowings", "Cannot delete a book with copies currently borrowed."));
            }

            _bookRepository.Remove(book);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
