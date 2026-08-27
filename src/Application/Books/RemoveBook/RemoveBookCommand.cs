using Application.Abstractions.Messaging;

namespace Application.Books;

public sealed record RemoveBookCommand(Guid Id) : ICommand;
