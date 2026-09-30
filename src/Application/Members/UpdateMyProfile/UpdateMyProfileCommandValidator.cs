using FluentValidation;

namespace Application.Members.UpdateMyProfile;

public sealed class UpdateMyProfileCommandValidator : AbstractValidator<UpdateMyProfileCommand>
{
    public UpdateMyProfileCommandValidator()
    {
        RuleFor(req => req.PhoneNumber)
            .Length(10).WithMessage("Phone number must be exactly 10 characters long.")
            .When(req => !string.IsNullOrWhiteSpace(req.PhoneNumber));
    }
}
