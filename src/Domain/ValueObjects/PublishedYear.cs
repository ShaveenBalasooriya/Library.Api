using Domain.Enums;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed class PublishedYear : ValueObject
{
    public int Value { get; init; }

    private PublishedYear(int value) => Value = value;

    public static Result<PublishedYear> Create(int yearValue)
    {
        if (yearValue <= 0)
        {
            return Result<PublishedYear>.Failure(new Error("PublishedYear.Negative", "Published year must be greater than zero.", ErrorType.Validation));
        }

        if (yearValue > DateTime.UtcNow.Year)
        {
            return Result<PublishedYear>.Failure(new Error("PublishedYear.InFuture", "Published year cannot be in the future.", ErrorType.Validation));
        }

        return Result<PublishedYear>.Success(new PublishedYear(yearValue));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
