namespace Application.Books;

public sealed record BookResponse(
    Guid Id,
    string Title,
    string Author,
    string Isbn,
    int PublishedYear,
    int TotalCopies,
    int AvailableCopies);
