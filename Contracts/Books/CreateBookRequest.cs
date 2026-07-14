using System.ComponentModel.DataAnnotations;

namespace Library.Api.Contracts.Books;

public record class CreateBookRequest(
    [Required] string Title,
    [Required] string Author,
    [Required] string Isbn,
    int PublishedYear,
    [Range(1, int.MaxValue)]int TotalCopies
);