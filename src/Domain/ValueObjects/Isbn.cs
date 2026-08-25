using Domain.Shared;
using Domain.Primitives;

namespace Domain.ValueObjects;

public sealed class Isbn : ValueObject
{
    public string Value { get; init; }

    private Isbn(string value) => Value = value;

    public static Result<Isbn> Create(string isbnValue)
    {
        if (string.IsNullOrWhiteSpace(isbnValue))
        {
            return Result<Isbn>.Failure(new Error("Isbn.Empty", "ISBN cannot be empty"));
        }

        if (isbnValue.Length != 10 && isbnValue.Length != 13)
        {
            return Result<Isbn>.Failure(new Error("Isbn.Invalid", "ISBN must be 10 or 13 characters long."));
        }

        return Result<Isbn>.Success(new Isbn(isbnValue));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
