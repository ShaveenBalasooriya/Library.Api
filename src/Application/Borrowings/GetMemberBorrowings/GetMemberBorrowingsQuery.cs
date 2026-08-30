using Application.Abstractions.Messaging;

namespace Application.Borrowings;

public sealed record GetMemberBorrowingsQuery(Guid MemberId) : IQuery<IReadOnlyList<BorrowingResponse>>;
