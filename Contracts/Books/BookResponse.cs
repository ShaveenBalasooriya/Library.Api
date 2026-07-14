namespace Library.Api.Contracts.Books;

public record class BookResponse(
    Guid Id,
    string Title,
    string Author,
    string Isbn,
    int PublishedYear,
    int TotalCopies,
    int AvailableCopies
);