using FluentValidation;

namespace Application.Books.AddBook;

public sealed class AddBookCommandValidator : AbstractValidator<AddBookCommand>
{
    public AddBookCommandValidator()
    {
        RuleFor(req => req.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

        RuleFor(req => req.Author)
            .NotEmpty().WithMessage("Author is required.")
            .MaximumLength(100).WithMessage("Author cannot exceed 100 characters.");

        RuleFor(req => req.Isbn)
            .NotEmpty().WithMessage("ISBN is required.")
            .Must(isbn => isbn.Length == 10 || isbn.Length == 13)
            .WithMessage("ISBN must be 10 or 13 characters long.")
            .When(req => !string.IsNullOrWhiteSpace(req.Isbn));

        RuleFor(req => req.PublishedYear)
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("Published year cannot be in the future.");

        RuleFor(req => req.TotalCopies)
            .GreaterThan(0)
            .WithMessage("Total copies must be at least 1.");
    }
}
