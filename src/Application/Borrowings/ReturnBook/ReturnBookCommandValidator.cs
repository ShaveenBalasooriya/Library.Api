using FluentValidation;

namespace Application.Borrowings.ReturnBook;

public sealed class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
{
    public ReturnBookCommandValidator()
    {
        RuleFor(req => req.BorrowingId)
            .NotEmpty().WithMessage("Borrowing Id is required.");
    }
}
