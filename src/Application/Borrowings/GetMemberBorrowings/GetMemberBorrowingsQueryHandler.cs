using Application.Abstractions.Messaging;
using Application.Members;
using Domain.Enums;
using Domain.Shared;

namespace Application.Borrowings
{
    internal sealed class GetMemberBorrowingsQueryHandler : IQueryHandler<GetMemberBorrowingsQuery, IReadOnlyList<BorrowingResponse>>
    {
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IMemberRepository _memberRepository;

        public GetMemberBorrowingsQueryHandler(IBorrowingRepository borrowingRepository, IMemberRepository memberRepository)
        {
            _borrowingRepository = borrowingRepository;
            _memberRepository = memberRepository;
        }

        public async Task<Result<IReadOnlyList<BorrowingResponse>>> Handle(GetMemberBorrowingsQuery request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken);
            if (member is null)
            {
                return Result<IReadOnlyList<BorrowingResponse>>.Failure(new Error("Member.NotFound", $"Member with ID '{request.MemberId}' was not found.", ErrorType.NotFound));
            }

            var borrowings = await _borrowingRepository.GetByMemberIdAsync(request.MemberId, cancellationToken);

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
