namespace Library.Api.Endpoints.Books;

public sealed record UpdateBookRequest(
    string Title,
    string Author,
    string Isbn,
    int PublishedYear,
    int TotalCopies);
