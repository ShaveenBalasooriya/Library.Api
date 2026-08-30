using FluentValidation;

namespace Application.Borrowings.BorrowBook;

public sealed class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
{
    public BorrowBookCommandValidator()
    {
        RuleFor(req => req.BookId)
            .NotEmpty().WithMessage("Book Id is required.");

        RuleFor(req => req.MemberId)
            .NotEmpty().WithMessage("Member Id is required.");
    }
}
