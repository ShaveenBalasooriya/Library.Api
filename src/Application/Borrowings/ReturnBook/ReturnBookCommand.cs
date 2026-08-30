using Application.Abstractions.Messaging;

namespace Application.Borrowings;

public sealed record ReturnBookCommand(Guid BorrowingId) : ICommand;
