using Application.Abstractions.Messaging;

namespace Application.Borrowings;

public sealed record GetAllBorrowingsQuery : IQuery<IReadOnlyList<BorrowingResponse>>;
