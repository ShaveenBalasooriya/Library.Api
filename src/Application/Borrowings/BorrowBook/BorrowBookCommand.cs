using Application.Abstractions.Messaging;

namespace Application.Borrowings;

public sealed record BorrowBookCommand(Guid BookId, Guid MemberId) : ICommand<Guid>;
