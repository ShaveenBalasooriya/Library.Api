namespace Domain.Shared;

public sealed record class Error(string code, string error)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}
