using FluentValidation;

namespace Application.Members.GetMember;

public sealed class GetMemberQueryValidator : AbstractValidator<GetMemberQuery>
{
    public GetMemberQueryValidator()
    {
        RuleFor(req => req.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
