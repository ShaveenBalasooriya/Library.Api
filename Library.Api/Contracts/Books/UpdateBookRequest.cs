using System.ComponentModel.DataAnnotations;

namespace Library.Api.Contracts.Books;

public record class UpdateBookRequest(
    [Required] string Title,
    [Required] string Author,
    int PublishedYear,
    [Range(1, int.MaxValue)]int TotalCopies
);
