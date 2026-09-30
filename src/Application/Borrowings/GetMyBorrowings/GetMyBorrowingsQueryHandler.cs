using Application.Abstractions.Messaging;
using Application.Members;
using Domain.Shared;

namespace Application.Borrowings
{
    internal sealed class GetMyBorrowingsQueryHandler : IQueryHandler<GetMyBorrowingsQuery, IReadOnlyList<BorrowingResponse>>
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly ICurrentMemberProvider _currentMemberProvider;

        public GetMyBorrowingsQueryHandler(IBorrowingRepository borrowingRepository, ICurrentMemberProvider currentMemberProvider)
        {
            _borrowingRepository = borrowingRepository;
            _currentMemberProvider = currentMemberProvider;
        }

        public async Task<Result<IReadOnlyList<BorrowingResponse>>> Handle(GetMyBorrowingsQuery request, CancellationToken cancellationToken)
        {
            var currentMemberResult = await _currentMemberProvider.GetOrCreateCurrentMemberAsync(cancellationToken);
            if (currentMemberResult.IsFailure)
            {
                return Result<IReadOnlyList<BorrowingResponse>>.Failure(currentMemberResult.Error);
            }

            var borrowings = await _borrowingRepository.GetByMemberIdAsync(currentMemberResult.Value.Id, cancellationToken);

            var response = borrowings
                .Select(borrowing => new BorrowingResponse(
                    borrowing.Id,
                    borrowing.BookId,
                    borrowing.MemberId,
                    borrowing.BorrowedDate,
                    borrowing.DueDate,
                    borrowing.ReturnedDate,
                    borrowing.Status))
                .ToList();

            return Result<IReadOnlyList<BorrowingResponse>>.Success(response);
        }
    }
}
