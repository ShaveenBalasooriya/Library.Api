using Domain.Enums;

namespace Domain.Shared;

public sealed record class Error(string Code, string Message, ErrorType ErrorType)
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
}
