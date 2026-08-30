using Domain.Enums;

namespace Application.Borrowings;

public sealed record BorrowingResponse(
    Guid Id,
    Guid BookId,
    Guid MemberId,
    DateTime BorrowedDate,
    DateTime DueDate,
    DateTime? ReturnedDate,
    BorrowingStatus Status);
