using FluentValidation;

namespace Application.Members.UpdateMember;

public sealed class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    public UpdateMemberCommandValidator()
    {
        RuleFor(req => req.Id)
            .NotEmpty().WithMessage("Id is required.");

        RuleFor(req => req.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(200).WithMessage("Full name cannot exceed 200 characters.");

        RuleFor(req => req.PhoneNumber)
            .Length(10).WithMessage("Phone number must be exactly 10 characters long.")
            .When(req => !string.IsNullOrWhiteSpace(req.PhoneNumber));
    }
}
