using FluentValidation;

namespace Application.Borrowings.BorrowBook;

public sealed class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
{
    public BorrowBookCommandValidator()
    {
        RuleFor(req => req.BookId)
            .NotEmpty().WithMessage("Book Id is required.");

        RuleFor(req => req.MemberId)
            .NotEqual(Guid.Empty).WithMessage("Member Id cannot be empty.")
            .When(req => req.MemberId.HasValue);
    }
}
