using Application.Abstractions.Messaging;

namespace Application.Books;

public sealed record GetBookQuery(Guid Id) : IQuery<BookResponse>;
