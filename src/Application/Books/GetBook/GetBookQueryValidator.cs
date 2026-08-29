using FluentValidation;

namespace Application.Books.GetBook;

public sealed class GetBookQueryValidator : AbstractValidator<GetBookQuery>
{
    public GetBookQueryValidator()
    {
        RuleFor(req => req.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
