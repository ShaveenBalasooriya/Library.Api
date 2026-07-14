using System.ComponentModel.DataAnnotations;

namespace Library.Api.Contracts.Borrowings;

public record class BorrowingBookRequest(
    [Required] Guid BookId,
    [Required] Guid MemberId
);
