using Application.Abstractions.Messaging;

namespace Application.Borrowings;

public sealed record GetMyBorrowingsQuery : IQuery<IReadOnlyList<BorrowingResponse>>;
