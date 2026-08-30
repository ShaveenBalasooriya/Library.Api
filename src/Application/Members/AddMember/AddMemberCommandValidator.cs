using FluentValidation;

namespace Application.Members.AddMember;

public sealed class AddMemberCommandValidator : AbstractValidator<AddMemberCommand>
{
    public AddMemberCommandValidator()
    {
        RuleFor(req => req.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(200).WithMessage("Full name cannot exceed 200 characters.");

        RuleFor(req => req.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email address format is invalid.");

        RuleFor(req => req.PhoneNumber)
            .Length(10).WithMessage("Phone number must be exactly 10 characters long.")
            .When(req => !string.IsNullOrWhiteSpace(req.PhoneNumber));
    }
}
