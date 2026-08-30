using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Books;
using Application.Members;
using Domain.Entities;
using Domain.Enums;
using Domain.Shared;

namespace Application.Borrowings
{
    internal sealed class BorrowBookCommandHandler : ICommandHandler<BorrowBookCommand, Guid>
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BorrowBookCommandHandler(
            IBorrowingRepository borrowingRepository,
            IBookRepository bookRepository,
            IMemberRepository memberRepository,
            IUnitOfWork unitOfWork)
        {
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken);
            if (member is null)
            {
                return Result<Guid>.Failure(new Error("Member.NotFound", $"Member with ID '{request.MemberId}' was not found.", ErrorType.NotFound));
            }

            var book = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken);
            if (book is null)
            {
                return Result<Guid>.Failure(new Error("Book.NotFound", $"Book with ID '{request.BookId}' was not found.", ErrorType.NotFound));
            }

            if (!member.IsActive)
            {
                return Result<Guid>.Failure(new Error("Member.NotActive", "Member does not have an active membership and cannot borrow books.", ErrorType.Forbidden));
            }

            var activeBorrowings = await _borrowingRepository.GetActiveBorrowingsAsync(request.MemberId, cancellationToken);

            var canBorrowResult = Borrowing.EnsureMemberCanBorrow(activeBorrowings.Count);
            if (canBorrowResult.IsFailure)
            {
                return Result<Guid>.Failure(canBorrowResult.Error);
            }

            var borrowCopyResult = book.BorrowCopy();
            if (borrowCopyResult.IsFailure)
            {
                return Result<Guid>.Failure(borrowCopyResult.Error);
            }

            var borrowingResult = Borrowing.Create(book.Id, member.Id);
            if (borrowingResult.IsFailure)
            {
                return Result<Guid>.Failure(borrowingResult.Error);
            }

            var borrowing = borrowingResult.Value;

            _bookRepository.Update(book);
            _borrowingRepository.Add(borrowing);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(borrowing.Id);
        }
    }
}
