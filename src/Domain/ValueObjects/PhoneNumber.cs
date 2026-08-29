using Domain.Enums;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    public string Value { get; }

    private PhoneNumber(string value) => Value = value;

    public Result<PhoneNumber> Create(string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            return Result<PhoneNumber>.Failure(new Error("PhoneNumber.Empty", "Phone number cannot be empty.", ErrorType.Validation));
        }

        if (phoneNumber.Length != 10)
        {
            return Result<PhoneNumber>.Failure(new Error("PhoneNumber.InvalidLength", "Phone number must be exactly 10 characters long.", ErrorType.Validation));
        }

        return Result<PhoneNumber>.Success(new PhoneNumber(phoneNumber));
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}
