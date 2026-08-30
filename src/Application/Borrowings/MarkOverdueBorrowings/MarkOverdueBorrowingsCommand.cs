using Application.Abstractions.Messaging;

namespace Application.Borrowings;

public sealed record MarkOverdueBorrowingsCommand : ICommand<int>;
