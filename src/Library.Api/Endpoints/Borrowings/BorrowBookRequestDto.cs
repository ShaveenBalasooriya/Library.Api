namespace Library.Api.Endpoints.Borrowings;

public sealed record BorrowBookRequestDto(Guid BookId, Guid MemberId);
