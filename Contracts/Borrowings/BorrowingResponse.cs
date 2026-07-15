namespace Library.Api.Contracts.Borrowings;

public record class BorrowingResponse(
    Guid Id,
    Guid BookId,
    Guid MemberId,
    DateTime BorrowedDate,
    DateTime DueDate,
    DateTime? ReturnedDate,
    string Status
);