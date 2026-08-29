namespace Library.Api.Endpoints.Books;

public sealed record BookRequestDto(
    string Title,
    string Author,
    string Isbn,
    int PublishedYear,
    int TotalCopies);
