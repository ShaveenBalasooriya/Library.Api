using Application.Abstractions.Messaging;

namespace Application.Books;

public sealed record GetAllBooksQuery : IQuery<IReadOnlyList<BookResponse>>;
