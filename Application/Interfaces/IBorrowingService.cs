using Library.Api.Contracts.Borrowings;

namespace Library.Api.Application.Interfaces;

public interface IBorrowingService
{
    Task<IReadOnlyList<BorrowingResponse>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<BorrowingResponse>> GetByMemberIdAsync(Guid memberId, CancellationToken ct = default);
    Task<BorrowingResponse> BorrowAsync(BorrowingBookRequest request, CancellationToken ct = default);
    Task<BorrowingResponse> ReturnAsync(Guid borrowingId, CancellationToken ct = default);
}
