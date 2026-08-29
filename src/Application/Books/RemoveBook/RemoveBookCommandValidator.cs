using FluentValidation;

namespace Application.Books.RemoveBook;

public sealed class RemoveBookCommandValidator : AbstractValidator<RemoveBookCommand>
{
    public RemoveBookCommandValidator()
    {
        RuleFor(req => req.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
