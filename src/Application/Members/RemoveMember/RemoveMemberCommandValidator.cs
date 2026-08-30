using FluentValidation;

namespace Application.Members.RemoveMember;

public sealed class RemoveMemberCommandValidator : AbstractValidator<RemoveMemberCommand>
{
    public RemoveMemberCommandValidator()
    {
        RuleFor(req => req.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
