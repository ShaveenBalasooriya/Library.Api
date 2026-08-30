using FluentValidation;

namespace Application.Borrowings.GetMemberBorrowings;

public sealed class GetMemberBorrowingsQueryValidator : AbstractValidator<GetMemberBorrowingsQuery>
{
    public GetMemberBorrowingsQueryValidator()
    {
        RuleFor(req => req.MemberId)
            .NotEmpty().WithMessage("Member Id is required.");
    }
}
