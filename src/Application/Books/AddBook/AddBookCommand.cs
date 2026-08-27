using Application.Abstractions.Messaging;

namespace Application.Books;

public sealed record AddBookCommand(
    string Title,
    string Author,
    string Isbn,
    int PublishedYear,
    int TotalCopies) : ICommand<Guid>;
