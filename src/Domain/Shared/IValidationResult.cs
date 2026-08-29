using Domain.Enums;

namespace Domain.Shared;

public interface IValidationResult
{
    Error[] Errors { get; }

    public static readonly Error ValidationError = new(
        "ValidationError",
        "One or more validation errors occurred",
        ErrorType.Validation);
}
