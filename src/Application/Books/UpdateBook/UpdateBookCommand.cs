using Application.Abstractions.Messaging;

namespace Application.Books;

public sealed record UpdateBookCommand(
    Guid Id,
    string Title,
    string Author,
    string Isbn,
    int PublishedYear,
    int TotalCopies) : ICommand;
