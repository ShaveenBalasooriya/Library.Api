using Application.Abstractions.Messaging;
using Domain.Shared;

namespace Application.Borrowings
{
    internal sealed class GetAllBorrowingsQueryHandler : IQueryHandler<GetAllBorrowingsQuery, IReadOnlyList<BorrowingResponse>>
    {
        private readonly IBorrowingRepository _borrowingRepository;

        public GetAllBorrowingsQueryHandler(IBorrowingRepository borrowingRepository)
        {
            _borrowingRepository = borrowingRepository;
        }

        public async Task<Result<IReadOnlyList<BorrowingResponse>>> Handle(GetAllBorrowingsQuery request, CancellationToken cancellationToken)
        {
            var borrowings = await _borrowingRepository.GetAllAsync(cancellationToken);

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
